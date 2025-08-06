using UnityEngine;
using TMPro;

public class InteractionText : BasicUI
{
    [SerializeField] private TextMeshProUGUI mainText;

    public void Open(string text)
    {
        mainText.SetText(text);
        Open();
    }
}
