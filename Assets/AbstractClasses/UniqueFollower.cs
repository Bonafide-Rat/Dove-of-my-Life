using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UniqueFollower : MonoBehaviour
{
    [Tooltip("Force Applied on x-axis when thrown")]
    public float throwPowerForward;
    [Tooltip("Force Applied on y-axis when thrown")]
    public float throwPowerUp;
    [Tooltip("Time until follower returns to player")]
    public float cooldown;

    private Rigidbody2D rb;
    private Collider2D collider;

    public void Throw()
    {
        rb.velocity = new Vector2(throwPowerForward, throwPowerUp);
    }
    public abstract void UseAbility();
}
