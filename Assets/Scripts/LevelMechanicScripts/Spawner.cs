using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    [SerializeField] FollowPath followPathObject;
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameManagerScript gameManager;
    private bool isTriggered;

    private void Start()
    {
        gameManager.OnRespawn += ResetTrigger;
    }

    void OnTriggerEnter2D(Collider2D other)
    {    
        //Debug.Log(isTriggered);
        if (!other.CompareTag("Player")) return;
        if (!followPathObject.gameObject.activeSelf)
        {
            //Debug.Log("Entering trigger..");
            followPathObject.gameObject.SetActive(true);
            followPathObject.ResetToInitialWaypoint(spawnPosition.position);
        }

        if (isTriggered) return;
        followPathObject.doMoveChaser = true;
        //followPathObject.paused = false;
        isTriggered = true;
       // Debug.Log(followPathObject.doMoveChaser  + " " + followPathObject.paused);
    }

    private void ResetTrigger()
    {
        isTriggered = false;
    }
}
