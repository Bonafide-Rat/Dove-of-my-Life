using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAcrossScreen : MonoBehaviour
{
    public float speed = 1f; // Speed of movement

    private void Start()
    {
        int environmentLayer = LayerMask.NameToLayer("Environment");
        if (environmentLayer >= 0 && environmentLayer <= 31)
        {
            // Ignore collisions with objects on the layer tagged as "Environment"
            Physics.IgnoreLayerCollision(gameObject.layer, environmentLayer);
        }
    }

    private void Update()
    {
        // Calculate the movement vector
        Vector3 movement = Vector3.right * speed * Time.deltaTime;

        // Update the position of the GameObject
        transform.position += movement;
    }
}


