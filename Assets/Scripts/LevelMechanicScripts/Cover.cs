using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<SquashAndStretch>() != null)
        {
            Debug.Log("Player In Cover");
            GameManagerScript.playerInCover = true;
            other.gameObject.GetComponent<SquashAndStretch>().Sprite.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f,0.3f,1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<SquashAndStretch>() != null)
        {
            Debug.Log("Player Out Cover");
            GameManagerScript.playerInCover = false;
            other.gameObject.GetComponent<SquashAndStretch>().Sprite.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
