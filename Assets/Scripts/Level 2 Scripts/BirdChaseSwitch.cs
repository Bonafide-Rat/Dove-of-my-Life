using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdChaseSwitch : MonoBehaviour
{
    
    [SerializeField] GameObject Active;
    [SerializeField] GameObject inActive;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collider)
    { 
        if (collider.CompareTag("Player"))
        {
           Active.SetActive(false);
           inActive.SetActive(true);
        }
    }
}
