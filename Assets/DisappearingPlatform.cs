using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    private Collider2D platCollider2D;
    private Animator animator;
    private Grid grid;
    private bool isPlayerOffPlatform;
    private bool isTriggered;
    // Start is called before the first frame update

    void Start()
    {
        animator = GetComponent<Animator>();
        platCollider2D = GetComponent<Collider2D>();
        grid = GetComponentInChildren<Grid>();
        Debug.Log(grid);
        GameManagerScript.OnRespawn += StartReset;
    }
    private void OnDestroy()
    {
        GameManagerScript.OnRespawn -= StartReset;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("GroundCheck"))
        {
            Debug.Log("exit");
            grid.enabled = false;
            animator.enabled = false;
            platCollider2D.enabled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("GroundCheck"))
        {
            Debug.Log("exit");
            grid.enabled = false;
            animator.enabled = false;
            platCollider2D.enabled = false;
        }
    }

    private void StartReset()
    {
        grid.enabled = true;
        animator.enabled = true;
        platCollider2D.enabled = true;
    }
}
