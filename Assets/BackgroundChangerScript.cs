using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundChangerScript : MonoBehaviour
{
    [SerializeField] private GameObject backgroundToActive;

    [SerializeField] private AudioManager audioManager;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            backgroundToActive.SetActive(!backgroundToActive.activeSelf);
            if (audioManager != null)
            {
                audioManager.SetSecondBGM();
            }
        }
    }
}
