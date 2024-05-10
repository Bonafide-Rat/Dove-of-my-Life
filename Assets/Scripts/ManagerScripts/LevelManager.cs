using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static float TargetScore;
    [SerializeField]private int setTargetScore;
    [SerializeField] private Image scoreFillbar;
    [SerializeField] private Image scoreBorder;
    [SerializeField] private Sprite maxScoreBorder;
    private bool maxScoreReached;
    public static Image PollinationBar;
    public static float CurrentScore;


    // Start is called before the first frame update
    void Start()
    {
        PollinationBar = scoreFillbar;
        TargetScore = setTargetScore;
        CurrentScore = 0;
        scoreFillbar.fillAmount = CurrentScore;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!maxScoreReached && CurrentScore >= TargetScore)
        {
            scoreBorder.sprite = maxScoreBorder;
            maxScoreReached = true;
        }
    }

    public static void AddScore(int amount)
    {
        if (CurrentScore < TargetScore)
        {
            CurrentScore += amount;
            UpdateUI();
        }
    }

    public static void UpdateUI()
    {
        PollinationBar.fillAmount = CurrentScore / TargetScore;
    }
}
