using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int TargetScore;
    [SerializeField]private int setTargetScore;
    [SerializeField] private TextMeshProUGUI scoretext;
    public static int CurrentScore;


    // Start is called before the first frame update
    void Start()
    {
        TargetScore = setTargetScore;
        CurrentScore = 0;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    public static void AddScore(int amount)
    {
        if (CurrentScore < TargetScore)
        {
            CurrentScore += amount;
        }
    }

    private void UpdateUI()
    {
        scoretext.text = $"{CurrentScore}/{TargetScore}";
    }
}
