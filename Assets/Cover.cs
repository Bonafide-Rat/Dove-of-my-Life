using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManagerScript.playerInCover = true;
            Debug.Log(GameManagerScript.playerInCover);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManagerScript.playerInCover = false;
            Debug.Log(GameManagerScript.playerInCover);
        }
    }
}
