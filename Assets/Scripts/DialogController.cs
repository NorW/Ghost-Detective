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
    
    public Sprite barDefault;
    public Sprite highDefault;
    public Sprite dDefault;
    public Sprite lovDefault;


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

    public void SetActive( bool active )
    {
        diBack.SetActive( active );
    }

    public bool IsActive()
    {
        return diBack.activeSelf;
    }

    public void SetDialogue( string dialogue )
    {
        dbDialog.GetComponent< TextMeshProUGUI >().text = dialogue;
    }

    public void SetName( string name )
    {
        dbName.GetComponent<TextMeshProUGUI>().text = name;
    }

    public void SetOptions(string option1, string option2 )
    {
        foText.GetComponent<TextMeshProUGUI>().text = option1;
        soText.GetComponent<TextMeshProUGUI>().text = option2;
        firstOption.SetActive(true);
        secondOption.SetActive(true);
    }


    public void HideOptions()
    {
        firstOption.SetActive( false );
        secondOption.SetActive( false );
    }

    public void SetIcon(string character)
    {
        if (character == "Ghost Detective")
        {
            nonGDIcon.GetComponent<Image>().color = new Color32(56, 56, 56, 255);
            GDIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            nonGDIcon.SetActive(true);
            GDIcon.SetActive(true);
        }
        else if (character == "Death")
        {
            nonGDIcon.GetComponent<Image>().sprite = dDefault;
            GDIcon.GetComponent<Image>().color = new Color32(56, 56, 56, 255);
            nonGDIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            nonGDIcon.SetActive(true);
            GDIcon.SetActive(true);
        }
        else if (character == "Les")
        {
            nonGDIcon.GetComponent<Image>().sprite = lovDefault;
            GDIcon.GetComponent<Image>().color = new Color32(56, 56, 56, 255);
            nonGDIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            nonGDIcon.SetActive(true);
            GDIcon.SetActive(true);
        }
        else if (character == "Barry, The Barista")
        {
            nonGDIcon.GetComponent<Image>().sprite = barDefault;
            GDIcon.GetComponent<Image>().color = new Color32(56, 56, 56, 255);
            nonGDIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            nonGDIcon.SetActive(true);
            GDIcon.SetActive(true);
        }
        else if (character == "Les")
        {
            nonGDIcon.GetComponent<Image>().sprite = highDefault;
            GDIcon.GetComponent<Image>().color = new Color32(56, 56, 56, 255);
            nonGDIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            nonGDIcon.SetActive(true);
            GDIcon.SetActive(true);
        }
        else
        {
            GDIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            nonGDIcon.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }
        
    }
    
}
