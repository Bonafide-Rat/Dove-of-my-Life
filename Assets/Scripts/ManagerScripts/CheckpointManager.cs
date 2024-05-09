using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckpointManager : MonoBehaviour
{
    public GameManagerScript gameManager;
    public bool isUpdateNextEnemyResetPoint;
    private bool isTriggered;
    public bool setsMusic;
    public MusicManager musicManager;
    public AudioClip musicToPlay;

    private void Awake()
    {
        // gameManager = GetComponent<GameManagerScript>(); // not necessary at the moment - do not uncomment.
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            gameManager.UpdateCheckpoint(transform.position, isUpdateNextEnemyResetPoint);
            isTriggered = true;
            if (setsMusic)
            {
                StartCoroutine(musicManager.SetMusic(musicToPlay, 3f));
            }
        }
    }

}
