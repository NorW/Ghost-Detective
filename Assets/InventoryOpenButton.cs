using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryOpenButton : MonoBehaviour, IPointerClickHandler
{
    InventoryViewBase inventory = null;

    // Start is called before the first frame update
    void Start()
    {
        inventory = gameObject.GetComponentInParent<InventoryViewBase>();
        
        if( inventory == null )
        {
            gameObject.SetActive( false );
            Debug.LogError( "Inventory not found" );
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if( inventory != null )
        {
            inventory.ToggleOpen();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
