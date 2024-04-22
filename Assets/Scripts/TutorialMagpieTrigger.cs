using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMagpieTrigger : MonoBehaviour
{
    public MagpieController magpieController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        magpieController.StartMagpie();
    }
}
