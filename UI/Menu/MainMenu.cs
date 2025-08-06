using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private GameObject settingsPanel;
    private void Awake()
    {
        playButton.onClick.AddListener(Play);
        settingsButton.onClick.AddListener(() => settingsPanel.SetActive(true));
        backToMenuButton.onClick.AddListener(() => settingsPanel.SetActive(false));
    }
    public void Play() => SceneManager.LoadScene("MainScene");
    

}
