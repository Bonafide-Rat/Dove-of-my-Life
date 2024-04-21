using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    [SerializeField] private GameObject endEffects;
    // Start is called before the first frame update
    void Start()
    {
        endEffects.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            endEffects.SetActive(true);
            Destroy(gameObject);
        }
    }
}
