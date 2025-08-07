using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] InteractionText interactionText;
    [SerializeField] InteractionText dialogText;
    private void Awake()
    {
        CloseInteractionText();
        CloseDialogText();
    }

    public void SetInteractionText(string text) => interactionText.Open(text);
    public void CloseInteractionText() => interactionText.Close();

    public void SetDialogText(string text) => dialogText.Open(text);
    public void CloseDialogText() => dialogText.Close();
}
