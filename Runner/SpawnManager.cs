using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstacleprefabArray;
    public GameObject[] hardObstaclesPrefabsArray;
    private Vector3 spawnPos = new Vector3(70, 0, 0);
    private float startDelay = 4.0f;
    private float timeInterval = 2.0f;
    private PlayerController playerControllerScript;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRandomObstacle", startDelay, timeInterval);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    public void UpdateSpeed(float speed)
    {
        foreach (var obs in obstacleprefabArray) obs.GetComponent<MoveLeft>().speed = speed;
        foreach (var obs in hardObstaclesPrefabsArray) obs.GetComponent<MoveLeft>().speed = speed;
    }

    void SpawnRandomObstacle()
    {
        if (playerControllerScript.gameOver == false)
        {
            if (gameManager.score > 100 && (Random.Range(0f,1f) > ((600 - (float) gameManager.score) / 500)))
            {
                int hardObstacleIndex = Random.Range(0, hardObstaclesPrefabsArray.Length);
                Instantiate(hardObstaclesPrefabsArray[hardObstacleIndex], spawnPos, obstacleprefabArray[hardObstacleIndex].transform.rotation);
                return;
            }
            int obstacleIndex = Random.Range(0, obstacleprefabArray.Length);
            Instantiate(obstacleprefabArray[obstacleIndex], spawnPos, obstacleprefabArray[obstacleIndex].transform.rotation);
        }
    }
}
