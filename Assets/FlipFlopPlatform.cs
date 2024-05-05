using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipFlopPlatform : MonoBehaviour
{

    [SerializeField] private HingeJoint2D joint2D;
    private JointMotor2D jointMotor;
    [SerializeField] private float switchBuffer;
    private bool isPlayerOffPlatform;
    private float switchBufferCache;
    // Start is called before the first frame update
    void Start()
    {
        jointMotor = joint2D.motor;
        switchBufferCache = switchBuffer;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOffPlatform)
        {
            HandleMotor();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && joint2D.limitState is JointLimitState2D.LowerLimit or JointLimitState2D.UpperLimit)
        {
            isPlayerOffPlatform = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Touch");
        if (other.gameObject.CompareTag("Player"))
        {
            switchBuffer = switchBufferCache;
            isPlayerOffPlatform = false;
        }
    }

    private void HandleMotor()
    {
        switchBuffer -= Time.deltaTime;
        if (!(switchBuffer <= 0)) return;
        jointMotor.motorSpeed *= -1;
        joint2D.motor = jointMotor;
        isPlayerOffPlatform = false;
        switchBuffer = switchBufferCache;
    }
}
