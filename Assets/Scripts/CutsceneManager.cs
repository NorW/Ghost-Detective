using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutsceneAction
{
    protected bool isComplete = false;

    public bool IsComplete { get { return isComplete; } }

    public abstract bool PlayAction();

    public virtual bool Update( float dt ) { return true; }
}


public class CutsceneSceneChange : CutsceneAction
{
    public override bool PlayAction()
    {
        throw new System.NotImplementedException();
    }

    public override bool Update( float dt )
    {
        throw new System.NotImplementedException();
    }
}

public class CutsceneMoveCamera : CutsceneAction
{
    private Vector3 position;
    private float speed;

    public CutsceneMoveCamera( Vector3 destination, float moveSpeed )
    {
        position = destination;
        speed = moveSpeed;
    }

    public override bool PlayAction()
    {
        isComplete = false;

        return isComplete;
    }

    public override bool Update( float dt )
    {
        //TODO get camera and move 
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
        throw new System.NotImplementedException();
        return isComplete;
    }
}

public class CutsceneManager : MonoBehaviour
{
    private static CutsceneManager instance = null;

    public static CutsceneManager Instance { get { return instance; } }


    [SerializeField] TextAsset cutsceneData;

    private Vector3 cameraOriginalPosition;

    private List<CutsceneAction> cutsceneActionQueue;
    private int curCutsceneAction = 0;

    private Dictionary<string, List<CutsceneAction>> cutscenes = new Dictionary<string, List<CutsceneAction>>();

    void Awake()
    {
        if ( instance == null )
        {
            instance = this;
            LoadCutsceneData();
        }
    }

    void LoadCutsceneData()
    {

    }

    public void StartCutscene( string cutscene )
    {
        //TODO set cameraOriginalPosition
        cutsceneActionQueue = cutscenes[ cutscene ];
        curCutsceneAction = 0;
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

