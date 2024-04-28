using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLineSpin : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    void Update()
    {
        var newXRot = transform.rotation.x + spinSpeed;
        var newyRot = transform.rotation.y + spinSpeed;
        
        transform.Rotate(spinSpeed * Time.deltaTime, spinSpeed * Time.deltaTime,0);
    }
}
