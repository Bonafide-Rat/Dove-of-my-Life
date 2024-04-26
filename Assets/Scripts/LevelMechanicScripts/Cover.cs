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
            other.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f,0.3f,1);
            other.gameObject.GetComponent<Collider2D>().excludeLayers = 1 << 11;
            Debug.Log(GameManagerScript.playerInCover);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManagerScript.playerInCover = false;
            other.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            other.gameObject.GetComponent<Collider2D>().includeLayers = 1 << 11;
            Debug.Log(GameManagerScript.playerInCover);
        }
    }
}
