using TMPro;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    [SerializeField]
    private string _areaTitle;

    private Animator _areaAnimator;
    private TextMeshProUGUI _areaTitleText;

    private void Awake()
    {
        GameObject areaUI = GameObject.Find("AreaUI");
        _areaAnimator = areaUI.GetComponent<Animator>();
        // Get the text, which is a child of a child of AreaUI (AreaUI → Background → Area Title)
        _areaTitleText = areaUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {   // If the player enters the collider, change the text of the area title, and start the animation
        if (other.tag == "Player")
        {
            _areaTitleText.text = _areaTitle;
            _areaAnimator.Play("AreaUIAppear");
        }
    }
}
