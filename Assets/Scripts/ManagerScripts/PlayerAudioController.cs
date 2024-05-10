using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAudioController : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] clips;


    private void Update()
    {
        if (audioSource.isPlaying)
        {
            MovePitchForWalking();
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Jump()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clips[0], 0.5f);
    }

    public void MovePitchForWalking()
    {
        audioSource.pitch = Random.Range(0.7f, 1.1f);
    }

    public void PlayAudio()
    {
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
