using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

public class DialogueSystem : MonoBehaviour
{
    private static DialogueSystem instance = null;

    [ SerializeField ] TextAsset dialogue;

    private Dictionary<string, DialogueNode> dialogueTrees = null;

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

    public DialogueNode StartDialogue( string conversationTarget )
    {
        return dialogueTrees[ conversationTarget ];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum DialogueNodeType
{
    Top,
    Dialogue,
    Choice,
    Prompt
}

public enum DialogueTagType
{
    INVALID,
    Alias,
    Required,
    Gives,
    GOTO,
    Text,
    Owner
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
}