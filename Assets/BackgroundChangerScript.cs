using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundChangerScript : MonoBehaviour
{
    [SerializeField] private List<Sprite> replacementImages = new();
    [SerializeField] private List<GameObject> backgroundToReplace = new();
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var panel in backgroundToReplace)
            {
                panel.GetComponent<Image>().sprite = replacementImages[backgroundToReplace.IndexOf(panel)];
            }
        }
    }
}
