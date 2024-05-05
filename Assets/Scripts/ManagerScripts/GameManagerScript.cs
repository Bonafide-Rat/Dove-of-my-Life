using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] FollowPath FollowPathObject;

    public GameObject gameOverUi;
    public GameObject levelPassedUi;
    public TextMeshProUGUI scoreText;
    private int maxScore;
  
    public static bool playerInCover;

    public Vector2 checkpointPos;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        checkpointPos = transform.position;
        PlayerController.isFacingRight = true;
        playerInCover = false;
        gameOverUi.SetActive(false);
        levelPassedUi.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameOver(){
        gameOverUi.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LevelPassed()
    {
        scoreText.text =
            $"Level Complete. You added: {FollowerManager.followerCount} / {maxScore} guests to the Wedding.";
        levelPassedUi.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UpdateCheckpoint(Vector2 newPos)
    {
        checkpointPos = newPos;
        Debug.Log("Player checkpoint updated. ");
    }

    public void respawn()
    {
        Player.transform.position = checkpointPos;
        FollowPathObject.ResetToLastWaypoint(); // Reset the path of the following object
    }
}
