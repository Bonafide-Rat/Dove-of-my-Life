using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Follower"))
        {
            LevelManager.AddScore(1);
            animator.SetTrigger("Triggered");
        }
      
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
