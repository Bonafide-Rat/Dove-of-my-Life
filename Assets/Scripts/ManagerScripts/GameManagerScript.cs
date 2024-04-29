using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUi;
    public GameObject levelPassedUi;
    public TextMeshProUGUI scoreText;
    private int maxScore;
    

    public static bool playerInCover;

    // Start is called before the first frame update
    void Start()
    {
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
}
