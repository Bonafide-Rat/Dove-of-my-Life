using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] FollowPath FollowPathObject;

    public GameObject gameOverUi;
    public GameObject levelPassedUi;
    public GameObject pausedUI;
    public TextMeshProUGUI scoreText;
    public static bool GamePaused;
    public static bool playerInCover;
    private bool playerIsResetting;
    public float timeToResetPosition;
    public Vector2 checkpointPos;

    public delegate void OnRespawnDelegate();

    public static event OnRespawnDelegate OnRespawn;

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            respawn();
        }

        if (!Input.GetKeyDown(KeyCode.Escape) || SceneManager.GetActiveScene().buildIndex == 0) return;
        PauseUnpauseGame();
    }

    public void gameOver(){
        gameOverUi.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LevelPassed()
    {
        if (LevelManager.TargetScore > 0){
        scoreText.text =
            $"Level Complete. \n {LevelManager.CurrentScore} / {LevelManager.TargetScore} flowers pollinated.";
        levelPassedUi.SetActive(true);
        Time.timeScale = 0f;
        }
    }

    public void UpdateCheckpoint(Vector2 newPos, bool updateNextPause)
    {
        checkpointPos = newPos;
        FollowPathObject.resetIndex = FollowPathObject.waypointIndex;
        if (FollowPathObject.pauseWaypoints.Any() && !FollowPathObject.paused)
        {
            Debug.Log("PauseCheck");
            if (FollowPathObject.nextPauseWaypoint != FollowPathObject.pauseWaypoints.Length && updateNextPause)
            {
                FollowPathObject.nextPauseWaypoint += 1;
            }
            FollowPathObject.resetIndex = Array.IndexOf(FollowPathObject.waypoints, FollowPathObject.pauseWaypoints[FollowPathObject.nextPauseWaypoint]);
            FollowPathObject.transform.position = FollowPathObject.waypoints[FollowPathObject.resetIndex].transform.position;
        }
    }

    public void respawn()
    {
        //Player.transform.position = checkpointPos;
        if (playerIsResetting) return;
        OnRespawn?.Invoke();

        
        
        StartCoroutine(MovePlayerToCheckpoint());
        // FollowPathObject.ResetToLastWaypoint(); // Reset the path of the following object
        if (FollowPathObject != null)
        {
            FollowPathObject.ResetToInitialWaypoint(checkpointPos);
        }
    }

    IEnumerator MovePlayerToCheckpoint()
    {
        if (Player == null) yield break;
        playerIsResetting = true;
        Collider2D[] playerColliders = Player.GetComponentsInChildren<Collider2D>();

        foreach (var collider in playerColliders)
        {
            collider.enabled = false;
        }

        Vector3 playerInitalPosition = Player.transform.position;
        float resetProgress = 0f;

        while (resetProgress < timeToResetPosition)
        {
            resetProgress += Time.deltaTime;
            Player.transform.position =
                Vector3.Lerp(playerInitalPosition, checkpointPos, resetProgress / timeToResetPosition);
            yield return null;
        }

        Player.transform.position = checkpointPos;
        foreach (var collider in playerColliders)
        {
            collider.enabled = true;
        }

        playerIsResetting = false;

    }

    public void PauseUnpauseGame()
    {
        if (GamePaused == false)
        {
            Time.timeScale = 0;
            pausedUI.SetActive(true);
            GamePaused = true;
        }
        else
        {
            Time.timeScale = 1;
            pausedUI.SetActive(false);
            GamePaused = false;
        }
        
    }


}
