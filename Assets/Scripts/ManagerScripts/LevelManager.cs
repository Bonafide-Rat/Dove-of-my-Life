using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int TargetScore;
    [SerializeField]private int setTargetScore;

    public static int CurrentScore;
    // Start is called before the first frame update
    void Start()
    {
        TargetScore = setTargetScore;
        CurrentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddScore(int amount)
    {
        if (CurrentScore < TargetScore)
        {
            CurrentScore += amount;
        }
        
        Debug.Log("Current Score: " + CurrentScore + "/" + TargetScore);
    }
}
