using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{

    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private SettingsPanel settingsPanel;
    private void Awake()
    {
        playButton.onClick.AddListener(Play);
        settingsButton.onClick.AddListener(settingsPanel.Open);
    }
    public void Play() => SceneManager.LoadScene("MainScene");
    

}
