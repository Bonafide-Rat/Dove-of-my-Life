using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public GameManagerScript gameManager;

    private void Awake()
    {
        // gameManager = GetComponent<GameManagerScript>(); // not necessary at the moment - do not uncomment.
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.UpdateCheckpoint(transform.position);
        }
    }

}
