using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Openable : Interactable
{
    public Sprite open;
    public Sprite closed;

    private SpriteRenderer sr;
    private bool isOpen;

    [SerializeField] string dialogueName;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closed;
    }

    public override void Interact()
    {
        if (isOpen)
        {
            sr.sprite = closed;
        }
        else
        {
            sr.sprite = open;
        }

        isOpen = !isOpen;

        DialogueSystem.Instance.StartDialogue( dialogueName );
    }



}
