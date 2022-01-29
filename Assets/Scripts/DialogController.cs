using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogController : MonoBehaviour
{
    public GameObject diBack; //dialog background
    public GameObject nonGDIcon; //the icon positioned to the left of the screen
    public GameObject GDIcon; //the icon positioned to the right of the screen that should usually be GD
    public GameObject dbName; //name textbox
    public GameObject dbDialog; //dialog textbox, apparently all textmeshpro text boxes are gameobjects and not. text.
    public GameObject firstOption; //Option Button #1, sure hope it counts as a gameobject
    public GameObject foText; //The Text that goes on the first option button
    public GameObject secondOption; //Option Button #2
    public GameObject soText; //The text that goes on the second option button. Maybe I'm going a bit overboard with these comments
    public GameObject pressButton; //Not sure if dialog will be progessing automatically or if youll have to hit a button so for now I have text that cones up

    void Start()
    {
        //initialize a bunch of stuff to Death Cutscene
    }

    
    void Update()
    {
        //Check for changes made to dialog box
        if( Input.GetKeyDown( KeyCode.KeypadEnter ) )
        {
            DialogueSystem.Instance.StartDialogue( "Opening" );
        }

        if( Input.GetKeyDown( KeyCode.X) )
        {
            DialogueSystem.Instance.NextLine();
        }
    }

    public void Dialog()
    {
        //Check for which dialog tree bit the game is at
        //Dialog box and icons will always be enabled, icons just need to be set
        //Set Background image to the correct background
        //Set the icons of the characters present
        //Set the name of the character speaking
        //Enable either dialog text box or the two options with the necessary text
        //Either automatically progress to next chunk of dialog or enable player to press a button to move forward
        //Loop for next chunk of dialog?
    }

    public void SetDialogue( string dialogue )
    {
        dbDialog.GetComponent< TextMeshProUGUI >().text = dialogue;
    }

    public void SetName( string name )
    {
        dbName.GetComponent<TextMeshProUGUI>().text = name;
    }
}
