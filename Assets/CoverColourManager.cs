using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverColourManager : MonoBehaviour
{
    private SpriteRenderer playerRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HidePlayer()
    {
        if (playerRenderer.color.Equals(Color.white))
        {
            playerRenderer.color = new Color(0x807272);
        }
    }
}
