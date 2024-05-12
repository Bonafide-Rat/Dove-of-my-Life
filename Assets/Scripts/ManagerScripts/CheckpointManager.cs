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
    [SerializeField] private float musicTransitionTime;
    [Range(0,1)]
    [SerializeField] private float musicVolumeSet;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            audioManager.PlaySFX(audioManager.checkpoint);
            gameManager.UpdateCheckpoint(transform.position, isUpdateNextEnemyResetPoint);
            isTriggered = true;
        }
    }

}
