using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button2Controller : MonoBehaviour
{
    public DialogueSystem dsystem;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener( OnClick );
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
    {
        DialogueSystem.Instance.NextLine( 1);
    }
}
