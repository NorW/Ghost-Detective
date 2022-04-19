using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPrototypeManager : MonoBehaviour
{
    [SerializeField] InventoryViewBase prototype1, prototype2, prototype3, prototype4;
    [SerializeField] InventoryViewBase currentPrototype = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.Alpha1 ) )
        {

        }
        else if( Input.GetKeyDown( KeyCode.Alpha2 ) )
        {

        }
        else if( Input.GetKeyDown( KeyCode.Alpha3 ) )
        {

        }
        else if( Input.GetKeyDown( KeyCode.Alpha4 ) )
        {

        }
    }
}
