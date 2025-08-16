using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReplayScreen : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private TextMeshProUGUI touchGrass;
    [SerializeField] private GameObject replayText;
    public UnityEvent OnReplay;
    private void Awake()
    {
        OnReplay = new();
        replayButton.onClick.AddListener(OnReplay.Invoke);
        gameObject.SetActive(false);
        touchGrass.gameObject.SetActive(false);
    }

    public void GoTouchGrass(bool success)
    {
        replayButton.interactable = false;
        if (!success) touchGrass.SetText("Wow you've played a lot of games ! For your mental health, you should go outside and touch some grass...");
        touchGrass.gameObject.SetActive(true);
        replayText.SetActive(false);
    }
}
