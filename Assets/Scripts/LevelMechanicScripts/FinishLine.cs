using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameManagerScript gameManager;

    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    { 
        if (collider.CompareTag("Player") && LevelManager.CurrentScore == LevelManager.TargetScore)
        {
            audioManager.PlaySFX(audioManager.levelComplete);
            gameManager.LevelPassed();
        }
        else
        {
            Debug.Log("Need " + (LevelManager.TargetScore - LevelManager.CurrentScore) + " more pollinations.");
        }
    }
}
