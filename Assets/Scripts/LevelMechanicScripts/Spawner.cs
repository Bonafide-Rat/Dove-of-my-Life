using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] FollowPath FollowPathObject;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Entering trigger..");
            FollowPathObject.gameObject.SetActive(true);
            FollowPathObject.ResetToInitialWaypoint(other.transform.position);
        }
    }
}
