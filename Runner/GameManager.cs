using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public ReplayScreen replayScreen;
    public GameObject tutorialPanel;
    public Button startButton;
    public decimal notScore;
    public decimal score;
    private int numberOfGames;
    private PlayerController playerControllerScript;

    public Transform startingPoint;
    private float lerpSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        score = 0;
        notScore = 0;
        playerControllerScript.gameOver = true;
        playerControllerScript.OnGameOver.AddListener(GameOver);
        replayScreen.OnReplay.AddListener(Replay);
        startButton.onClick.AddListener(() => StartCoroutine(PlayIntro()));
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_f", 0);
        tutorialPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.gameOver)
        {
            if (playerControllerScript.doubleSpeed)
            {
                notScore += 5;
            }

            else
            {
                notScore++;
            }
            if (decimal.Round(notScore / 20) > 300 && score < 300) UpdateSpeed(30);
            score = decimal.Round(notScore / 20);
            scoreText.SetText("Score :  " + score);
        }
    }

    IEnumerator PlayIntro()
    {
        tutorialPanel.SetActive(false);
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_f", 0);
        yield return new WaitForSeconds(2);
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_f", 1);
        Vector3 startPos = playerControllerScript.transform.position;
        Vector3 cinematicEnd = startingPoint.position;
        float cinematicDistance = cinematicEnd.x - startPos.x;
        float startTime = Time.time;
        float distanceCovered = 0;

        while (distanceCovered < cinematicDistance)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            playerControllerScript.transform.position = Vector3.Lerp(startPos, cinematicEnd, distanceCovered / cinematicDistance);
            playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier", distanceCovered / cinematicDistance);
            yield return null;
        }
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier", 1.0f);
        playerControllerScript.gameOver = false;

    }
    private void UpdateSpeed(float speed)
    {
        foreach (var obj in FindObjectsByType<MoveLeft>(FindObjectsSortMode.None)) { obj.speed = speed; }
        GameObject.Find("SpawnManager").GetComponent<SpawnManager>().UpdateSpeed(speed);
    }
    private void Replay()
    {
        FindFirstObjectByType<RepeatBackground>().InitializePosition();
        foreach (var obj in FindObjectsByType<MoveLeft>(FindObjectsSortMode.None)) { obj.ResetObstacles(); }
        playerControllerScript.ResetPlayer();
        replayScreen.gameObject.SetActive(false);
        score = 0;
        notScore = 0;
        scoreText.SetText("Score :" + score);
        tutorialPanel.SetActive(true);
    }
    private IEnumerator StopGame(bool replay)
    {
        if (replay)
        {
            replayScreen.OnReplay.AddListener(Replay);
        }
        else
        {
            replayScreen.GoTouchGrass(score >= 500);
        }
        yield return new WaitForSeconds(3);
        replayScreen.gameObject.SetActive(true);
        if (!replay)
        {
            Physics.gravity /= playerControllerScript.gravityModifier;
            SceneManager.LoadSceneAsync("GameScene");
        }
    }
    private void GameOver()
    {
        numberOfGames++;
        StartCoroutine(StopGame(score < 500 && numberOfGames < 6));
    }
}
