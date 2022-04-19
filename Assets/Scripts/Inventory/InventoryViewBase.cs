using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryViewBase : MonoBehaviour
{
    protected enum InventoryViewState
    {
        Closed,
        Closing,
        Opening,
        Open
    }

    [SerializeField] float openAnimationTime = 1.0f, closeAnimationTime = 1.0f;

    protected InventoryViewState viewState = InventoryViewState.Closed;
    protected float currentAnimationTime = 0.0f;

    protected void UpdateAnimation()
    {

    }

    public virtual void ToggleOpen()
    {

    }
}
