using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipFlopPlatform : MonoBehaviour
{

    [SerializeField] private HingeJoint2D joint2D;
    [SerializeField] private GameManagerScript gameManager;
    private JointMotor2D jointMotor;
    [SerializeField] private float switchBuffer;
    private bool isPlayerOffPlatform;
    private float switchBufferCache;
    private float startMotorSpeed;
    private float startAngle;
    private bool isResetting;
    // Start is called before the first frame update

    void Start()
    {
        startAngle = joint2D.jointAngle;
        jointMotor = joint2D.motor;
        startMotorSpeed = jointMotor.motorSpeed;
        switchBufferCache = switchBuffer;
        gameManager.OnRespawn += StartReset;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("CurrAngle: " + joint2D.jointAngle + " startAngle: " + startAngle);
    }

    private void FixedUpdate()
    {
        if (isPlayerOffPlatform)
        {
            HandleMotor(); 
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log(joint2D.jointAngle);
        if (other.gameObject.CompareTag("Player") && (joint2D.jointAngle <= 1 || Mathf.Approximately(joint2D.jointAngle, 180)))
        {
            Debug.Log("OffSuccess");
            isPlayerOffPlatform = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
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
        FlipPlatform();
    }

    private void ResetPlatform()
    {
        if (jointMotor.motorSpeed == startMotorSpeed) return;
        FlipPlatform();
        switchBuffer = switchBufferCache;
        isPlayerOffPlatform = false;
    }

    private void StartReset()
    {
        if (!isResetting)
        {
            StartCoroutine(ResetWithDelay());
        }
    }

    IEnumerator ResetWithDelay()
    {
        isResetting = true;
        yield return new WaitForSeconds(0.5f);
        ResetPlatform();
        isResetting = false;
    }

    private void FlipPlatform()
    {
        jointMotor.motorSpeed *= -1;
        joint2D.motor = jointMotor;
        isPlayerOffPlatform = false;
        switchBuffer = switchBufferCache;
    }
    
    private void SetJointLimits(float min, float max)
    {
        JointAngleLimits2D limits = joint2D.limits;
        limits.min = min;
        limits.max = max;
        joint2D.limits = limits;
    }
}
