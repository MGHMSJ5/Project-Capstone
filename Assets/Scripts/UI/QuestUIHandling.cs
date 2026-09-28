using System.Collections;
using TMPro;
using UnityEngine;

public class QuestUIHandling : MonoBehaviour
{
    private TextMeshProUGUI title;
    private TextMeshProUGUI description;

    private Animator animator;

    private bool questActive;

    public QuestInfoSO sideQuestInfo;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        title = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        description = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void StartQuest(string title, string description)
    {
        StartCoroutine(StartNewQuestWait(title, description));
    }

    private IEnumerator StartNewQuestWait(string title, string description)
    {
        while (questActive)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        questActive = true;
        animator.SetTrigger("QuestStart");
        this.title.text = title;
        this.description.text = description;
    }
    public void UpdateQuest(string title, string description)
    {
        animator.Play("QuestUpdated");
        this.title.text = title;
        this.description.text = description;
    }

    public void FinishedQuest()
    {
        animator.SetTrigger("QuestFinished");
    }

    public void FinishedQuestAnimationDone()
    {
        questActive = false;
    }
}
