using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    [SerializeField] FollowPath followPathObject;
    [SerializeField] Transform spawnPosition;
    private bool isTriggered;
    void OnTriggerEnter2D(Collider2D other)
    {    
        Debug.Log(isTriggered);
        if (!other.CompareTag("Player")) return;
        if (!followPathObject.gameObject.activeSelf)
        {
            //Debug.Log("Entering trigger..");
            followPathObject.gameObject.SetActive(true);
            followPathObject.ResetToInitialWaypoint(spawnPosition.position);
        }

        if (isTriggered) return;
        followPathObject.doMoveChaser = !followPathObject.doMoveChaser;
        //followPathObject.paused = false;
        isTriggered = true;
        Debug.Log(followPathObject.doMoveChaser  + " " + followPathObject.paused);
    }
}
