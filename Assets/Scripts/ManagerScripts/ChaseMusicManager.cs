using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMusicManager : MonoBehaviour
{
    private AudioManager audioManager;
    private bool isChasing;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isChasing)
        {
             Debug.Log("Entered Chase Region");
            // Pause whatever BGM, and play chase music.
            audioManager.PauseMusic();
            audioManager.PlayMusic(audioManager.chaseBGM, 1f);
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isChasing)
        {
             Debug.Log("Exiting and resetting music...");
            // Stop chase music and play regular BGM.
            audioManager.PauseMusic();
            audioManager.PlayMusic(audioManager.bgm, 1f);
            isChasing = false;
        }
    }
}
