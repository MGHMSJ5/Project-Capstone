using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class IntroSceneAnim : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private GameObject camera1;
    [SerializeField] private GameObject camera2;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        camera1.SetActive(true);
        camera2.SetActive(false);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraSwitchAnimation()
    {
        camera1.SetActive(false);
        camera2.SetActive(true);
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
    }
}
