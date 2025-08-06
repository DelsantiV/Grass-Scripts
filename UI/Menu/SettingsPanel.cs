using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : BasicUI
{

    [SerializeField] private Button backToMenuButton;



    
    private void Awake()
    {
        backToMenuButton.onClick.AddListener(Close);
    }
}
