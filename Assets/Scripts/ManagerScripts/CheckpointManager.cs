using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Vector2 checkpointPos;

    private void Start()
    {
        checkpointPos = transform.position;
    }

    public void UpdateCheckpoint(Vector2 newPos)
    {
        checkpointPos = newPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UpdateCheckpoint(transform.position);
        }
    }

}
