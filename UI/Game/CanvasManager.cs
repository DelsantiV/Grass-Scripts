using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] InteractionText interactionText;
    private void Awake()
    {
        
    }

    public void SetInteractionText(string text) => interactionText.Open(text);
    public void CloseInteractionText() => interactionText.Close();
}
