using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformToggleScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> platforms;
    [SerializeField] private float timer;
    [SerializeField] private bool isTimed;
    private bool isTriggered;
    private Animator animator;
    void Awake()
    {
        foreach (var platform in platforms) {
            platform.SetActive(false);
        }

        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Follower"))
        {
            if (isTimed && !animator.GetBool("UndoPlatform"))
            {
                TimerTogglePlatforms();
            }
            else
            {
                BasicPlatforms();
            }
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        
    }

    private void BasicPlatforms()
    {
        if (!isTriggered)
        {
            TogglePlatforms();
            animator.SetTrigger("Triggered");
            animator.SetBool("BasicPlatform", true);
            LevelManager.AddScore(1);
        }
    }

    private void TogglePlatforms()
    {
        foreach (var platform in platforms) {
            platform.SetActive(!platform.activeSelf);
        }
        animator.SetBool("UndoPlatform", false);
        isTriggered = true;
    }

    private void TimerTogglePlatforms()
    {
        if (!isTriggered)
        {
            LevelManager.AddScore(1);
        }
        TogglePlatforms();
        animator.SetTrigger("Triggered");
        animator.SetBool("UndoPlatform", true);
        Invoke(nameof(TogglePlatforms), timer);
        isTriggered = true;
    }

    private void FlashTimerPlatforms()
    {
        foreach (var platform in platforms)
        {
            if (platform.activeSelf)
            {
                var platformColour = platform.GetComponent<SpriteRenderer>().color;
                platformColour.a = Mathf.Lerp(1, 0, 10);
                platform.GetComponent<SpriteRenderer>().color = platformColour;
            }
        }
    }
}
