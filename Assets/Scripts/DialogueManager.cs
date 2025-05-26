using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Ink.Runtime;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject _nextDialogue;
    [SerializeField] private GameObject dialogueIndicator;
    private PlayerController _playerController;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] private TextMeshProUGUI[] choicesText;
    [SerializeField] private GameObject choicesPanel;

    private const string NAME_TAG = "Name";


    private Ink.Runtime.Story currentStory;

    public bool dialogueIsPlaying { get; private set;  } //outside scripts can read the value, but not modify it

    private static DialogueManager instance;
    private UIChangeSubject _UIChangeSubject;

    private void Awake()
    {
        if (instance != null)
        {
            //if more than one dialogue manager is in the scene, a warning will be shown. Multiple dialogue managers will crash the game.
            Debug.LogWarning("Found more than one DialogueManager in the scene!");
        }
        instance = this;
        //get the PlayerController script by looking for an object that has the "Player" tag, and then get the script component
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _UIChangeSubject = GameObject.Find("UIChangeManager").GetComponent<UIChangeSubject>();
    }

    private void OnEnable()
    {
        _UIChangeSubject.UISwitch += UIChange;
    }

    private void OnDisable()
    {
        _UIChangeSubject.UISwitch -= UIChange;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //find all of the text where the choices will show up in
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        //return right away if no dialogue is playing
        if (!dialogueIsPlaying)
        {
            return;
        }
        //if the interact button is pressed and there is no choice panel active, the game will go to the next line of dialogue (otherwise known as story)
        if (Input.GetButtonDown("Interact") && !choicesPanel.activeInHierarchy)
        {
            ContinueStory();
        }

        if (choicesPanel.activeInHierarchy)
        {
            dialogueIndicator.SetActive(false);
        }
        else
        {
            dialogueIndicator.SetActive(true);
        }
    }


    //player will enter the dialogue mode through the NPCInteract Script.
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        //start the dialogue by playing the first line of dialogue from the inkJSON file
        currentStory = new Ink.Runtime.Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();

        //disable the player movement (script)
        _playerController.enabled = false;

        //mouse cursor is visible and moveable
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitDialogueMode()
    {
        dialoguePanel.SetActive(false);
        //reset the text for reusability
        dialogueText.text = "";

        //enable the player movement (script)
        _playerController.enabled = true;
        dialogueIsPlaying = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void ContinueStory()
    {
        if (currentStory.canContinue) //checks if an inkJSON file with dialogue exists in the interacted NPC
        {
            //set text for the current dialogue line
            dialogueText.text = currentStory.Continue(); //finds the next line of dialogue
            //dislay choices, if those are part of the current dialogue line
            DisplayChoices();
            HandleTags(currentStory.currentTags);
        }
        else //if no inkJSON file, or no more dialogue in the inkJSON file
        {
            ExitDialogueMode();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            //spliting the tag in two part
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be split into two: " + tag);
            }
            string tagKey = splitTag[0].Trim(); //the first part of the tag is the key
            string tagValue = splitTag[1].Trim(); //the second part of the tag is the value

            switch (tagKey)
            {
                case NAME_TAG:
                    nameText.text = tagValue;
                    break;
                default:
                    Debug.LogWarning("Tag was not recognized by system: " + tagKey);
                    break;
            }
        }
    }

    private void DisplayChoices() //Nick: I get an error and Unity opens in safe mode because of this line!
                                  //Lea: I fixed the issue; for some reason I made this fuction a IEnumerator instead of a normal void fuction. It should work now!
    {
        //list all the choices given in the inkJSON file from the NPC
        List<Ink.Runtime.Choice> currentChoices = currentStory.currentChoices;

        //check to make sure the UI can handle the amount of choices
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given " + currentChoices.Count);
        }

        int index = 0;
        //enable the amount of needed choice buttons to be the amount of choices given by the inkJSON file
        foreach (Ink.Runtime.Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        //go through the remaining choices of the UI that aren't used for this line of dialogue, and make sure those are hidden

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        //select the first choice for controller users
        StartCoroutine(SelectFirstChoice());


    }

    private IEnumerator SelectFirstChoice()
    {
        //event System requires that the gameobject is first cleared, before it is changed to a current selected object. There must also be a little bit of waiting time in between the changes
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        //selects the first choice which is choice0
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    //if the player presses the button, this function starts
    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        //without this first continueStory, the screen stays the same, and therefore the dialogue system doesn't register the choice that the player makes, which creates errors
        ContinueStory();
        //this second continueStory is just for us. If we want to showcase the choice that the player made inside the dialogue panel, then we can delete this one.
        //using the design in the GDD, I made this decision.
        ContinueStory();
    }

    private void UIChange(bool isController)
    {
        foreach (GameObject t in choices)
        {
            t.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(isController);
        }

        _nextDialogue.transform.GetChild(0).gameObject.SetActive(!isController);
        _nextDialogue.transform.GetChild(1).gameObject.SetActive(isController);
    }
}
