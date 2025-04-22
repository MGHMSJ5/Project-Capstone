using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Ink.Runtime;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using JetBrains.Annotations;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private PlayerController _playerController;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] private TextMeshProUGUI[] choicesText;
    [SerializeField] private GameObject choicesPanel;


    private Ink.Runtime.Story currentStory;

    public bool dialogueIsPlaying { get; private set;  } //outside scripts can read the value, but not modify it

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one DialogueManager in the scene!");
        }
        instance = this;
        // Get the PlayerController script by looking for an object that has the "Player" tag, and then get the script component
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //get all of the choices text
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

        if (Input.GetButtonDown("Interact") && !choicesPanel.activeInHierarchy)
        {
            ContinueStory();
        }

        if (choicesPanel.activeInHierarchy)
        {
            if (Input.GetButton("DialogueChoice0"))
            {
                MakeChoice(0);
            }

            if (Input.GetButtonDown("DialogueChoice1"))
            {
                MakeChoice(1);
            }

            if (Input.GetButtonDown("DialogueChoice2"))
            {
                MakeChoice(2);
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Ink.Runtime.Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();

        // Disable the player movement (script)
        _playerController.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


    }

    public void ExitDialogueMode()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        // Enable the player movement (script)
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
            dialogueText.text = currentStory.Continue(); //Starts the first line of dialogue
            //dislay choices, if those are part of the current dialogue line
            DisplayChoices();
        }
        else //if no inkJSON file, or no more dialogue in the inkJSON file
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Ink.Runtime.Choice> currentChoices = currentStory.currentChoices;

        //check to make sure the UI can handle the amount of choices
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given " + currentChoices.Count);
        }

        int index = 0;
        //enable the choices to the amount of choices for this line of dialogue
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


    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires that the gameobject is first cleared, before it is changed to a current selected object. There must also be a little bit of waiting time in between the changes
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    private IEnumerator ControllerOptions(int buttoninput)
    {
        currentStory.ChooseChoiceIndex(buttoninput);
        //Without the first continueStory, the screen stays the same, and therefore the dialogue system doesn't register the choice that the player makes, which creates errors
        ContinueStory();
        //This second continueStory is just for us. If we want to showcase the choice that the player made inside the dialogue panel, then we can delete this one.
        //Going from the GDD, I made this decision, since this was the idea.
        ContinueStory();
        yield return new WaitForSeconds(0.5f);

    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        //Without the first continueStory, the screen stays the same, and therefore the dialogue system doesn't register the choice that the player makes, which creates errors
        ContinueStory();
        //This second continueStory is just for us. If we want to showcase the choice that the player made inside the dialogue panel, then we can delete this one.
        //Going from the GDD, I made this decision, since this was the idea.
        ContinueStory();
    }
}
