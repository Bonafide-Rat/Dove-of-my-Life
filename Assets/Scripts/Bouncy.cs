using System;
using System.Collections;
using System.Collections.Generic;
using AbstractClasses;
using UnityEngine;

public class Bouncy : UniqueFollower
{
    public override void UseAbility()
    {
       Debug.Log("Used");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }
}
