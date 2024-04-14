using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<BirdFollowers>().AddFollower();
            Destroy(gameObject);
        }
    }
}
