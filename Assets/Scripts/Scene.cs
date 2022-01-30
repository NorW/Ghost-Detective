using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] List<GameObject> playerSpawnPoints = new List<GameObject>();

    public string SceneName { get { return sceneName; } }

    public Vector3 GetSpawnPointPosition( int sceneIndex )
    {
        return playerSpawnPoints[ sceneIndex ].transform.position;
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
