using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformToggleScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> platforms;
    [SerializeField] private float timer;
    [SerializeField] private bool isTimed;
    void Start()
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
            else
            {
                BasicTogglePlatforms();
            }
        }
    }

    private void BasicTogglePlatforms()
    {
        foreach (var platform in platforms) {
            platform.SetActive(!platform.activeSelf);
        }
    }

    private void TimerTogglePlatforms()
    {
        BasicTogglePlatforms();
        Invoke(nameof(BasicTogglePlatforms), timer);
    }
}
