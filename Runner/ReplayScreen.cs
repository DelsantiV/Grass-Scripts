using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReplayScreen : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private GameObject touchGrass;
    public UnityEvent OnReplay;
    private void Awake()
    {
        OnReplay = new();
        replayButton.onClick.AddListener(OnReplay.Invoke);
        gameObject.SetActive(false);
        touchGrass.SetActive(false);
    }

    public void GoTouchGrass()
    {
        replayButton.interactable = false;
        touchGrass.SetActive(true);
    }
}
