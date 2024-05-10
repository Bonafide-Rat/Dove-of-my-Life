using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpeedChanger : MonoBehaviour
{
    [SerializeField] 
    public FollowPath FollowPath;

    public float ChangeSpeedTo;

    private void OnTriggerEnter2D(Collider2D collider)
    { 
        if (collider.CompareTag("Player") )
        {
            FollowPath.moveSpeed = ChangeSpeedTo;
        }
    }

}
