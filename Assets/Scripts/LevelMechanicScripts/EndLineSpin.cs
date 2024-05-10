using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLineSpin : MonoBehaviour
{
    [SerializeField] private float spinSpeed;

    private SpriteRenderer spriteRenderer;

    private Color originalColour;

    private bool hasChanged;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColour = spriteRenderer.color;
        float hue, saturation, value;
        Color.RGBToHSV(originalColour, out hue, out saturation, out value);
        Color greyscaleColour = Color.HSVToRGB(hue, 0, value);
        spriteRenderer.color = greyscaleColour;
    }

    void Update()
    {
        transform.Rotate(spinSpeed * Time.deltaTime, spinSpeed * Time.deltaTime,0);

        if (LevelManager.TargetScore == LevelManager.CurrentScore && !hasChanged)
        {
            spriteRenderer.color = originalColour;
            hasChanged = true;
        }
    }
    
    
}
