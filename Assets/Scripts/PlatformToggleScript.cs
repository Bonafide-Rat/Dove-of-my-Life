using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformToggleScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> platforms;
    [SerializeField] private float timer;
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Follower":
                BasicTogglePlatforms();
                Debug.Log("Hit");
                break;
        }
    }

    private void BasicTogglePlatforms()
    {
        if (!platforms[0].activeSelf)
        {
            foreach (var platform in platforms)
            {
                platform.SetActive(true);   
            }
        }
        else
        {
            foreach (var platform in platforms)
            {
                platform.SetActive(false);   
            }
        }
    }
}
