using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

public enum DialogueNodeType
{
    Top,
    Dialogue,
    Choice,
    Prompt,
    Cutscene
}

public enum DialogueTagType
{
    INVALID,
    Alias,
    Required,
    Gives,
    GOTO,
    Text,
    Owner,
    CutsceneStart,
    CutsceneContinue,
    VoiceLine
}

public class DialogueNode
{
    public DialogueNodeType type;
    public string[] tagsRequired = null;
    public string[] tagsGiven = null;
    public string owner;
    public string alias = null;
    public int nodeLevel;
    public DialogueNode parent = null;
    public List<DialogueNode> children = new List<DialogueNode>();
    public string goToTarget = null;
    public string dialogue = "";
    //public bool or enum display portrait? or int

    public string cutscene = null;
    public bool continueCutscene = false;
    public string voiceLine = null;
}

public class DialogueSystem : MonoBehaviour
{
    private static DialogueSystem instance = null;

    public static DialogueSystem Instance { get { return instance; } }

    [ SerializeField ] TextAsset dialogue;
    [ SerializeField] DialogController dialogueDisplay;

    private Dictionary<string, DialogueNode> dialogueTrees = null;

    private DialogueNode curNode = null;

    private void LoadDialogueTrees()
    {
        Assert.IsNotNull( dialogue );

        StringReader reader = new StringReader( dialogue.text );

        dialogueTrees = new Dictionary< string, DialogueNode >();

        DialogueNode curNode = null, curParent = null, prevNode = null;
        string line = null;

        while ( true )
        {
            line = reader.ReadLine();
            if ( line == null )
            {
                return;
            }

            curNode = ParseLine( line );

            if( curNode == null )
            {
                continue;
            }
            
            if( curNode.type == DialogueNodeType.Top )
            {
                curParent = curNode;
                curNode.parent = null;
                dialogueTrees.Add( curNode.owner, curNode );
            }
            else
            {
                if( prevNode == null )
                {
                    continue;   //Ill formed orphaned node
                }

                if( prevNode.nodeLevel < curNode.nodeLevel )
                {
                    curParent = prevNode;
                    curParent.children.Add( curNode );
                    curNode.parent = curParent;
                }
                else if( prevNode.nodeLevel == curNode.nodeLevel )
                {
                    curParent.children.Add( curNode );
                    curNode.parent = curParent;
                }
                else
                {
                    curNode.parent = GetParentOfLevel( prevNode, curNode.nodeLevel - 1 );
                    curNode.parent.children.Add( curNode );
                }
            }

            prevNode = curNode;
        }
    }

    private DialogueNode GetParentOfLevel( DialogueNode node, int level )
    {
        DialogueNode curNode = node;
        while( true )
        {
            curNode = curNode.parent;
            if( curNode == null )
            {
                return null; //unable to find suitable parent?? illformed data
            }

            if( curNode.nodeLevel == level )
            {
                return curNode;
            }
        }
    }

    private DialogueNode ParseLine( string line )
    {
        int nodeLevel = GetNodeLevel( line );
        if( nodeLevel == -1)
        {
            return null;
        }

        DialogueNode node = new DialogueNode();
        node.nodeLevel = nodeLevel;
        char type = line[ nodeLevel ];
        if ( nodeLevel == 0 )
        {
            if ( type == 't' )
            {
                node.type = DialogueNodeType.Top;
            }
            else
            {
                return null;
            }
        }
        else
        {
            if( type == 'd' )
            {
                node.type = DialogueNodeType.Dialogue;
            }
            else if( type == 'c' )
            {
                node.type = DialogueNodeType.Choice;
            }
            else if( type == 'p' )
            {
                node.type = DialogueNodeType.Prompt;
            }
            else if( type == 's' )
            {
                node.type = DialogueNodeType.Cutscene;
            }
            else
            {
                return null;
            }
        }

        DialogueTagType tag;
        int tagIndex = nodeLevel;

        while( true )
        {
            tag = GetNextTag( line, ref tagIndex );

            if( tagIndex == -1 )
            {
                return node;
            }

            switch( tag )
            {
                case DialogueTagType.Alias:
                    node.alias = GetName( line, ref tagIndex );
                    break;
                case DialogueTagType.Owner:
                    node.owner = GetName( line, ref tagIndex );
                    break;
                case DialogueTagType.Required:
                    node.tagsRequired = GetPlayerTags( line, ref tagIndex );
                    break;
                case DialogueTagType.Gives:
                    node.tagsGiven = GetPlayerTags( line, ref tagIndex );
                    break;
                case DialogueTagType.GOTO:
                    node.goToTarget = GetGOTO( line, ref tagIndex );
                    break;
                case DialogueTagType.Text:
                    node.dialogue = GetDialogue( line, ref tagIndex );
                    return node;
                case DialogueTagType.CutsceneStart:
                    node.cutscene = GetName( line, ref tagIndex );
                    break;
                case DialogueTagType.CutsceneContinue:
                    node.continueCutscene = true;
                    break;
                case DialogueTagType.VoiceLine:
                    node.voiceLine = GetName( line, ref tagIndex );
                    break;
            }

            if(tagIndex >= line.Length )
            {
                return node;
            }
        }
    }

    private string GetName( string line, ref int tagIndex )
    {
        int start = tagIndex;
        tagIndex = line.IndexOf( '$', start );

        if( tagIndex == -1 )
        {
            tagIndex = line.Length + 1;
        }

        return line.Substring( start, tagIndex - start - 1 ).Trim();
    }

    private string[] GetPlayerTags( string line, ref int tagIndex )
    {
        int start = tagIndex;
        tagIndex = line.IndexOf( '$', start );

        if( tagIndex == -1 )
        {
            tagIndex = line.Length;
        }

        return line.Substring( start, tagIndex - start - 1 ).Trim().Split( ',' );
    }

    private string GetGOTO( string line, ref int tagIndex )
    {
        int start = tagIndex;
        tagIndex = line.IndexOf( '$', start );

        if ( tagIndex == -1 )
        {
            tagIndex = line.Length;
        }

        return line.Substring( start, tagIndex - start - 1 ).Trim();
    }

    private string GetDialogue( string line, ref int tagIndex )
    {
        string dialogue = line.Substring( tagIndex );
        tagIndex = line.Length;
        return dialogue.Trim();
    }

    private DialogueTagType GetNextTag( string line, ref int index )
    {
        index = line.IndexOf( '$', index );
        
        if( index == -1 )
        {
            return DialogueTagType.INVALID;
        }

        if( ++index >= line.Length )
        {
            return DialogueTagType.INVALID;
        }

        switch( line[ index++ ] )
        {
            case 'n':
                return DialogueTagType.Alias;
            case 'r':
                return DialogueTagType.Required;
            case 'g':
                return DialogueTagType.Gives;
            case 's':
                return DialogueTagType.GOTO;
            case 't':
                return DialogueTagType.Text;
            case 'o':
                return DialogueTagType.Owner;
            case 'c':
                return DialogueTagType.CutsceneStart;
            case 'p':
                return DialogueTagType.CutsceneContinue;
            case 'v':
                return DialogueTagType.VoiceLine;
        }

        return DialogueTagType.INVALID;
    }

    private int GetNodeLevel( string line )
    {
        for(int i = 0; i < line.Length; i++ )
        {
            if( line[i] != '\t' )
            {
                return i;
            }
        }
        return -1;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy( this.gameObject );
        }
        else
        {
            instance = this;
            DontDestroyOnLoad( this.gameObject );
            LoadDialogueTrees();
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue( string conversationTarget )
    {
        curNode = dialogueTrees[ conversationTarget ];
        NextLine();
    }

    public void NextLine( int choice = 0 )
    {
        if ( curNode == null )
        {
            SetDialogueVisible( false );
            return;
        }

        //If curNode is a choice, switch to choice selected
        if ( curNode.type == DialogueNodeType.Choice || curNode.type == DialogueNodeType.Prompt )
        {
            curNode = curNode.parent.children[ choice ];
            SetDialogueVisible( true );
            dialogueDisplay.HideOptions();
        }

        if( curNode.goToTarget != null )
        {
            StartDialogue( curNode.goToTarget );
            return;
        }

        if ( curNode.children.Count == 0 )
        {
            SetDialogueVisible( false );
            return;
        }

        var player = PlayerState.Instance;

        foreach ( var node in curNode.children )
        {
            if ( player.HasAllTags( node.tagsRequired ) )
            {
                curNode = node;
                SetDialogueVisible( true );
                break;
            }
        }

        if ( curNode == null )
        {
            SetDialogueVisible( false );
            return;
        }

        player.AddTags( curNode.tagsGiven );
    
        switch( curNode.type )
        {
            case DialogueNodeType.Choice:
                dialogueDisplay.SetName( curNode.alias != null ? curNode.alias : curNode.owner );
                dialogueDisplay.SetOptions( curNode.parent.children[ 0 ].dialogue, curNode.parent.children[ 1 ].dialogue );
                break;

            case DialogueNodeType.Prompt:
                break;

            case DialogueNodeType.Dialogue:
                dialogueDisplay.SetName( curNode.alias != null ? curNode.alias : curNode.owner );
                dialogueDisplay.SetDialogue( curNode.dialogue );
                break;

            case DialogueNodeType.Cutscene:
                CutsceneManager.Instance.StartCutscene( curNode.cutscene );
                break;
        }

        if( curNode.voiceLine != null )
        {
            //TODO play voice line
        }
    }

    public void SetDialogueVisible( bool visible )
    {
        dialogueDisplay.SetActive( visible );
    }

    public bool IsDialogueVisible()
    {
        return dialogueDisplay.IsActive();
    }
}

