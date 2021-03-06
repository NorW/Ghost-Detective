using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance = null;

    public static SceneManager Instance { get { return instance; } }

    [SerializeField] List< Scene > scenes = new List< Scene >();
    [SerializeField] GameObject playerObject;

    private Dictionary< string, Scene > sceneLookup = new Dictionary< string, Scene >();

    void Awake()
    {
        if ( instance == null )
        {
            instance = this;
            foreach ( Scene scene in scenes )
            {
                sceneLookup.Add( scene.SceneName, scene );
            }
        }
    }

    public void MovePlayerToScene( string scene, int spawnPointIndex = 0 )
    {
        Assert.IsTrue( sceneLookup.ContainsKey( scene ) );
        Vector3 position = sceneLookup[ scene ].GetSpawnPointPosition( spawnPointIndex );

        playerObject.transform.position = position;
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
