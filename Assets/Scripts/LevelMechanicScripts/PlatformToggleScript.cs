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
    void Awake()
    {
        foreach (var platform in platforms) {
            platform.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Follower"))
        {
            if (isTimed)
            {
                TimerTogglePlatforms();
            }
            else if (!isTriggered)
            {
                BasicTogglePlatforms();
                LevelManager.AddScore(1);
            }
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        
    }

    private void BasicTogglePlatforms()
    {
        foreach (var platform in platforms) {
            platform.SetActive(!platform.activeSelf);
        }

        isTriggered = true;
    }

    private void TimerTogglePlatforms()
    {
        if (!isTriggered)
        {
            LevelManager.AddScore(1);
        }
        BasicTogglePlatforms();
        Invoke(nameof(BasicTogglePlatforms), timer);
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
