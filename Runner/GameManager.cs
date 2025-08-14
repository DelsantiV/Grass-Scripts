using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public ReplayScreen replayScreen;
    public GameObject tutorialPanel;
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
        StartCoroutine(PlayIntro());
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
            scoreText.SetText("Score :" + score);
        }
    }

    IEnumerator PlayIntro()
    {
        tutorialPanel.SetActive(true);
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_f", 0);
        yield return new WaitForSeconds(7);
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_f", 1);
        tutorialPanel.SetActive(false);
        Vector3 startPos = playerControllerScript.transform.position;
        Vector3 cinematicEnd = startingPoint.position;
        float cinematicDistance = cinematicEnd.x - startPos.x;
        float startTime = Time.time;
        float distanceCovered = 0;
        playerControllerScript.GetComponent<Animator>().SetFloat("Speed_Multiplier",0.5f);

        while (distanceCovered < cinematicDistance)
        {
            distanceCovered = (Time.time - startTime) * lerpSpeed;
            playerControllerScript.transform.position = Vector3.Lerp(startPos, cinematicEnd, distanceCovered / cinematicDistance);
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
        StartCoroutine(PlayIntro());
    }
    private IEnumerator StopGame(bool replay)
    {
        if (replay)
        {
            replayScreen.OnReplay.AddListener(Replay);
        }
        else
        {
            replayScreen.GoTouchGrass();
        }
        yield return new WaitForSeconds(3);
        replayScreen.gameObject.SetActive(true);
        if (!replay)
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadSceneAsync("GameScene");
        }
    }
    private void GameOver()
    {
        numberOfGames++;
        StartCoroutine(StopGame(score < 500 && numberOfGames < 10));
    }
}
