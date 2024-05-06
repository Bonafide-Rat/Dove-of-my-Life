using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] FollowPath FollowPathObject;
    private bool isTriggered;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!FollowPathObject.gameObject.activeSelf)
        {
            //Debug.Log("Entering trigger..");
            FollowPathObject.gameObject.SetActive(true);
            FollowPathObject.ResetToInitialWaypoint(other.transform.position);
        }
        if (!isTriggered)
        {
            FollowPathObject.doMoveChaser = !FollowPathObject.doMoveChaser;
            isTriggered = true;
        }
    }
}
