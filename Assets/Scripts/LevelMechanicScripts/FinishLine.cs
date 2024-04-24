using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameManagerScript gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    { 
        if (collider.CompareTag("Player") && LevelManager.CurrentScore == LevelManager.TargetScore)
        {
            gameManager.LevelPassed();
        }
        else
        {
            Debug.Log("Need " + (LevelManager.TargetScore - LevelManager.CurrentScore) + " more pollinations.");
        }
    }
}
