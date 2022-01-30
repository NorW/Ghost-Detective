using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public abstract class CutsceneAction
{
    protected bool isComplete = false;

    public bool IsComplete { get { return isComplete; } }

    public abstract bool PlayAction();

    public virtual bool Update( float dt ) { return true; }
}


public class CutsceneSceneChange : CutsceneAction
{
    private string sceneName;
    private int spawnPosition;
    private CutsceneNode node;

    public CutsceneSceneChange( CutsceneNode cutsceneNode, string sceneName, int spawnPositionIndex )
    {
        this.sceneName = sceneName;
        spawnPosition = spawnPositionIndex;
        node = cutsceneNode;
    }

    public override bool PlayAction()
    {
        //TODO fade in fade out
        SceneManager.Instance.MovePlayerToScene( sceneName, spawnPosition );
        Camera.main.transform.position = node.transform.position;
        Camera.main.orthographicSize = node.Size;
        return true;
    }

    public override bool Update( float dt )
    {
        return true;
    }
}

public class CutsceneMoveCamera : CutsceneAction
{
    private CutsceneNode node;
    private float time;

    private Vector3 positionDiff;
    private float sizeDiff;

    private float elapsedTime;

    public CutsceneMoveCamera( CutsceneNode destination, float timeToMove )
    {
        node = destination;
        time = timeToMove;
    }

    public override bool PlayAction()
    {
        isComplete = false;
        elapsedTime = 0.0f;
        positionDiff = node.transform.position - Camera.main.transform.position;
        sizeDiff = node.Size - Camera.main.orthographicSize;

        return isComplete;
    }

    public override bool Update( float dt )
    {
        elapsedTime += dt;

        float timeStep = dt / time;

        Camera.main.transform.position += positionDiff * timeStep;
        Camera.main.orthographicSize += sizeDiff * timeStep;

        if ( elapsedTime >= time )
        {
            Camera.main.transform.position = node.transform.position;
            Camera.main.orthographicSize = node.Size;
            isComplete = true;
        }

        return isComplete;
    }
}

//Releases control of cutscene to dialogue system.
public class CutsceneNextLine : CutsceneAction
{
    public override bool PlayAction()
    {
        DialogueSystem.Instance.NextLine();
        isComplete = false;
        return isComplete;
    }

    public override bool Update( float dt )
    {
        return DialogueSystem.Instance.IsDialogueVisible();
    }
}

public class CutsceneDialogueVisibility : CutsceneAction
{
    private bool toVisible;

    public CutsceneDialogueVisibility( bool visible )
    {
        toVisible = visible;
    }

    public override bool PlayAction()
    {
        DialogueSystem.Instance.SetDialogueVisible( toVisible );
        isComplete = true;
        return isComplete;
    }
}

public class CutscenePlayMusic : CutsceneAction
{
    private string track;

    public CutscenePlayMusic( string trackToPlay )
    {
        track = trackToPlay;
    }

    public override bool PlayAction()
    {
        //TODO
        throw new System.NotImplementedException();
        return isComplete;
    }
}

public class CutsceneStartDialogue : CutsceneAction
{
    private string dialogue;

    public CutsceneStartDialogue( string dialogueToStart )
    {
        dialogue = dialogueToStart;
    }

    public override bool PlayAction()
    {
        DialogueSystem.Instance.StartDialogue( dialogue );
        isComplete = true;
        return IsComplete;
    }
}

public class CutsceneManager : MonoBehaviour
{
    private static CutsceneManager instance = null;

    public static CutsceneManager Instance { get { return instance; } }


    [SerializeField] TextAsset cutsceneData;
    [SerializeField] SceneManager sceneManager;
    [SerializeField] List<CutsceneNode> cutsceneNodes;



    private Vector3 cameraOriginalPosition;

    private List<CutsceneAction> cutsceneActionQueue;
    private int curCutsceneAction = 0;

    private Dictionary<string, List<CutsceneAction>> cutscenes;
    private Dictionary<string, CutsceneNode> cutsceneNodeLookup = new Dictionary<string, CutsceneNode>();

    void Awake()
    {
        if ( instance == null )
        {
            instance = this;
            foreach( CutsceneNode node in cutsceneNodes )
            {
                cutsceneNodeLookup.Add( node.NodeName, node );
            }
            LoadCutsceneData();
        }
    }

    void LoadCutsceneData()
    {
        cutscenes = new Dictionary<string, List<CutsceneAction>>();

        StringReader reader = new StringReader( cutsceneData.text );

        string line;
        string cutsceneName = null;
        List<CutsceneAction> curList = new List<CutsceneAction>();

        while( true )
        {
            line = reader.ReadLine();
            if( line == null )
            {
                cutscenes.Add( cutsceneName, curList );
                break;
            }

            if( line.Length == 0 )
            {
                continue;
            }

            if( line[0] == '\t' )
            {
                var action = ParseAction( line );
                if ( action != null )
                {
                    curList.Add( action );
                }
            }
            else
            {
                if ( cutsceneName != null )
                {
                    cutscenes.Add( cutsceneName, curList );
                }

                cutsceneName = line.Trim();
                curList = new List< CutsceneAction >();
            }
        }
    }

    private CutsceneAction ParseAction( string raw )
    {
        int nameEnd = raw.IndexOf( ' ' );

        if( nameEnd == -1)
        {
            nameEnd = raw.Length - 1;
        }

        string name = raw.Substring( 1, nameEnd ).Trim().ToLower();
        CutsceneAction action = null;

        if( name == "dialoguevisibility" )
        {
            action = new CutsceneDialogueVisibility( raw.Substring( nameEnd ).Trim().ToLower() == "true" );
        }
        else if( name == "nextline" )
        {
            action = new CutsceneNextLine();
        }
        else if ( name == "movecamera" )
        {
            int nodeNameEndIndex = raw.IndexOf( ' ', raw[ nameEnd ] == ' ' ? nameEnd + 1 : nameEnd );
            string nodeName = raw.Substring( nameEnd, nodeNameEndIndex - nameEnd ).Trim();

            float time = 2.0f;

            if ( !float.TryParse( raw.Substring( nodeNameEndIndex ), out time ) || time <= 0.0f )
            {
                time = 2.0f;
            }

            if( !cutsceneNodeLookup.ContainsKey(nodeName) )
            {
                return null;
            }

            action = new CutsceneMoveCamera( cutsceneNodeLookup[ nodeName ], time );
        }
        else if ( name == "playmusic" )
        {
            action = new CutscenePlayMusic( raw.Substring( nameEnd ).Trim() );
        }
        else if ( name == "scenechange" )
        {
            int nameEndIndex = raw.IndexOf( ' ', raw[nameEnd] == ' ' ? nameEnd + 1 : nameEnd );
            string sceneName = raw.Substring( nameEnd, nameEndIndex - nameEnd ).Trim();

            int spawnPoint = 0;

            if ( !int.TryParse( raw.Substring( nameEndIndex ), out spawnPoint ) )
            {
                spawnPoint = 0;
            }

            if( !cutsceneNodeLookup.ContainsKey(sceneName) )
            {
                return null;
            }

            action = new CutsceneSceneChange( cutsceneNodeLookup[ sceneName ], sceneName, spawnPoint );
        }
        else if( name == "startdialogue")
        {
            action = new CutsceneStartDialogue( raw.Substring( nameEnd ).Trim() );
        }

        return action;
    }

    public void StartCutscene( string cutscene )
    {
        cameraOriginalPosition = Camera.main.transform.position;
        cutsceneActionQueue = cutscenes[ cutscene ];
        curCutsceneAction = 0;

        while( true )
        {
            if ( !cutsceneActionQueue[ curCutsceneAction ].PlayAction() )
            {
                break;
            }
            ++curCutsceneAction;

            if( curCutsceneAction >= cutsceneActionQueue.Count )
            {
                cutsceneActionQueue = null;
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if( cutsceneActionQueue == null)
        {
            return;
        }

        if( cutsceneActionQueue[ curCutsceneAction ].Update(Time.deltaTime ) )
        {
            while( true )
            {
                ++curCutsceneAction;

                if( curCutsceneAction >= cutsceneActionQueue.Count)
                {
                    cutsceneActionQueue = null;
                    break;
                }

                if( !cutsceneActionQueue[curCutsceneAction].PlayAction() )
                {
                    break;
                }
            }
        }
    }
}

