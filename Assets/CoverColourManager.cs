using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverColourManager : MonoBehaviour
{
    private SpriteRenderer playerRenderer;
    // Start is called before the first frame update
    void Start()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerScript.playerInCover)
        {
            HidePlayer();
        }
    }

    private void HidePlayer()
    {
        if (playerRenderer.color.Equals(Color.white))
        {
            playerRenderer.color = new Color(0.5f, 0.44f,0.44f,1);
        }
        else
        {
            playerRenderer.color = Color.white;
        }
    }
}
