using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideInventoryView : InventoryViewBase
{
    [SerializeField] float openXPosition, closedXPosition;

    Vector3 openPosition, closedPosition;

    RectTransform rectTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        openPosition = rectTransform.anchoredPosition;
        closedPosition = rectTransform.anchoredPosition;
        openPosition.x = openXPosition;
        closedPosition.x = closedXPosition;

        rectTransform.anchoredPosition = closedPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if ( viewState == InventoryViewState.Closed || viewState == InventoryViewState.Open )
        {
            return;
        }

        currentAnimationTime += Time.deltaTime;

        if ( viewState == InventoryViewState.Closing )
        {
            if ( currentAnimationTime >= closeAnimationTime )
            {
                rectTransform.anchoredPosition = closedPosition;
                viewState = InventoryViewState.Closed;
            }
            else
            {
                rectTransform.anchoredPosition = Vector3.Lerp( rectTransform.anchoredPosition, closedPosition, Mathf.Max( closeAnimationTime - currentAnimationTime, 0.2f ) / 60.0f );
            }
        }
        else if ( viewState == InventoryViewState.Opening )
        {
            if ( currentAnimationTime >= openAnimationTime )
            {
                rectTransform.anchoredPosition = openPosition;
                viewState = InventoryViewState.Open;
            }
            else
            {
                rectTransform.anchoredPosition = Vector3.Lerp( rectTransform.anchoredPosition, openPosition, Mathf.Max( openAnimationTime - currentAnimationTime, 0.2f ) / 60.0f );
            }
        }
    }

    public override void ToggleOpen()
    {
        switch ( viewState )
        {
            //Do nothing while in the middle of opening or closing
            case InventoryViewState.Closing:
                return;
            case InventoryViewState.Opening:
                return;

            case InventoryViewState.Closed:
                viewState = InventoryViewState.Opening;
                break;

            case InventoryViewState.Open:
                viewState = InventoryViewState.Closing;
                break;
        }

        currentAnimationTime = 0.0f;
    }
}
