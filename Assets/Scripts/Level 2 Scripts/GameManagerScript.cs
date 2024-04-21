using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUi;
    public GameObject levelPassedUi;

    public static bool playerInCover;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameOver(){
        gameOverUi.SetActive(true);
    }

    public void LevelPassed(){
        levelPassedUi.SetActive(true);
    }
}
