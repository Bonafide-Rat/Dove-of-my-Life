using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    public GameManagerScript gameManager;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !GameManagerScript.playerInCover) // Assuming the player has a tag of "Player"
        {
            //gameManager.gameOver();
            gameManager.respawn();
        }
    }
}
