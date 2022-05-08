using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryViewBase : MonoBehaviour
{
    protected enum InventoryViewState
    {
        Closed,
        Closing,
        Opening,
        Open
    }

    [SerializeField] protected float openAnimationTime = 1.0f, closeAnimationTime = 1.0f;

    protected InventoryViewState viewState = InventoryViewState.Closed;
    protected float currentAnimationTime = 0.0f;

    protected void UpdateAnimation()
    {

    }

    public abstract void ToggleOpen();

    public virtual void Activate()
    {
        gameObject.SetActive( true );
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive( false );
    }
}
