using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button1Controller : MonoBehaviour
{
    //public DialogueSystem dsystem;
    
    // Start is called before the first frame update
    void Start()
    {
        //dsystem = gameObject.GetComponent("DialogueSystem") as DialogueSystem;
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener( OnClick );
    }

    // Update is called once per frame
    void Update()
    {
        //not sure if needed
    }

    void OnClick()
    {
        DialogueSystem.Instance.NextLine(0);
    }
}
