using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindowView : InventoryViewBase
{
    [SerializeField] GameObject inventoryWrapper;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKeyDown( KeyCode.I ) )
        {
            ToggleOpen();
        }
    }

    public override void ToggleOpen()
    {
        inventoryWrapper.SetActive( !inventoryWrapper.activeSelf );
    }

    public override void Deactivate()
    {
        inventoryWrapper.SetActive( false );
        base.Deactivate();
    }

    public override void Activate()
    {
        inventoryWrapper.SetActive( false );
        base.Activate();
    }
}
