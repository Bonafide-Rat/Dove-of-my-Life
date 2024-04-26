using System;
using System.Collections;
using System.Collections.Generic;
using AbstractClasses;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bouncy : UniqueFollower
{
    [SerializeField]private float upOnBounce;
    public override void UseAbility()
    {
       Debug.Log("Used");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
       Rb.AddForce(Vector2.up * upOnBounce, ForceMode2D.Impulse);
       Rb.AddForce(Vector2.right * Random.Range(-10f,10f), ForceMode2D.Impulse);
    }
}
