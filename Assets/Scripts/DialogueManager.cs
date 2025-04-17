using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Ink.Runtime;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private PlayerController _playerController;

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
    }

    private void Update()
    {
        //return right away if no dialogue is playing
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetButtonDown("Interact"))
        {
            ContinueStory();
            print("Interacted with button");
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Ink.Runtime.Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        // Disable the player movement (script)
        _playerController.enabled = false;
        print("Entered Dialogue Modus");


    }

    public void ExitDialogueMode()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        // Enable the player movement (script)
        _playerController.enabled = true;
        print("exited Dialogue Modus");
        dialogueIsPlaying = false;
        //StartCoroutine(WaitForDialoguePanel());

    }

    private void ContinueStory()
    {
        if (currentStory.canContinue) //checks if an inkJSON file with dialogue exists in the interacted NPC
        {
            dialogueText.text = currentStory.Continue(); //Starts the first line of dialogue
            print("continued story");
        }
        else //if no inkJSON file, or no more dialogue in the inkJSON file
        {
            ExitDialogueMode();
            print("did not continue story");
        }
    }

    IEnumerator WaitForDialoguePanel()
    {
        yield return new WaitForSeconds(1f);
        dialogueIsPlaying = false;
    }

}
