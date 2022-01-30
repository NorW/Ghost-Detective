using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button1Controller : MonoBehaviour
{
    public DialogueSystem dsystem;
    
    // Start is called before the first frame update
    void Start()
    {
        dsystem = gameObject.GetComponent("DialogueSystem") as DialogueSystem;
    }

    // Update is called once per frame
    void Update()
    {
        //not sure if needed
    }

    void OnClick()
    {
        dsystem.NextLine(0);
    }
}
