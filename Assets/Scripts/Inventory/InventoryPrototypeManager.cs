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
        InventoryViewBase nextPrototype = null;

        if( Input.GetKeyDown( KeyCode.Alpha1 ) )
        {
            nextPrototype = prototype1;
        }
        else if( Input.GetKeyDown( KeyCode.Alpha2 ) )
        {
            nextPrototype = prototype2;
        }
        else if( Input.GetKeyDown( KeyCode.Alpha3 ) )
        {
            nextPrototype = prototype3;
        }
        else if( Input.GetKeyDown( KeyCode.Alpha4 ) )
        {
            nextPrototype = prototype4;
        }

        if( nextPrototype != null && nextPrototype != currentPrototype )
        {
            currentPrototype.Deactivate();
            currentPrototype = nextPrototype;
            currentPrototype.Activate();
        }
    }
}
