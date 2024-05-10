using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowerPickup : MonoBehaviour
{
    private AudioSource audioSource;
    [HideInInspector] public bool destroyOnPickup;


    private void Awake()
    {
        GameManagerScript.OnRespawn += Reset;
    }

    private void OnDestroy()
    {
        GameManagerScript.OnRespawn -= Reset;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<FollowerManager>() != null)
        {
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider2D other)
    {
        audioSource.pitch = Random.Range(0.65f, 1.1f);
        audioSource.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(audioSource.clip.length);
        other.GetComponent<FollowerManager>().AddFollower();
        GetComponent<AudioSource>().enabled = false;
        if (destroyOnPickup)
        {
            GameManagerScript.OnRespawn -= Reset;
            Destroy(gameObject);
        }
    }

    private void Reset()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<AudioSource>().enabled = true;
    }
}
