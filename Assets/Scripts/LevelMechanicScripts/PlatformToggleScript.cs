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
    [SerializeField]private bool isMakeAppear;
    private Animator animator;
    void Awake()
    {
        if (isMakeAppear)
        {
            foreach (var platform in platforms) {
                platform.SetActive(false);
            }
        }
        else
        {
            foreach (var platform in platforms) {
                platform.SetActive(true);
            }
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
        
        if (isTimed)
        {
            if (!isMakeAppear)
            {
                foreach (var platform in platforms)
                {
                    Debug.Log(platform.GetComponent<Animator>());
                    platform.GetComponent<Collider2D>().enabled = !platform.GetComponent<Collider2D>().enabled; 
                    platform.GetComponent<Animator>().SetBool("Triggered",!platform.GetComponent<Animator>().GetBool("Triggered"));
                }
            }
            else
            {
                foreach (var platform in platforms)
                {
                    Debug.Log(platform.GetComponent<Animator>());
                    platform.SetActive(!platform.activeSelf);
                    platform.GetComponent<Animator>().SetBool("Triggered",!platform.GetComponent<Animator>().GetBool("Triggered"));
                }
            }
        }
        else
        {
            foreach (var platform in platforms) {
                platform.SetActive(!platform.activeSelf);
            }
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
}
