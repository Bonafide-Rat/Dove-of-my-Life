using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResize : MonoBehaviour
{
    public float zoomOutSize = 20f;
    public float zoomSpeed = 5f;
    public Camera mainCamera;

    private bool playerEntered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playerEntered)
        {
            playerEntered = true;
            ZoomOutCamera();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = false;
        }
    }

    private void ZoomOutCamera()
    {
        StartCoroutine(ZoomOutCoroutine());
    }

    private IEnumerator ZoomOutCoroutine()
    {
        while (mainCamera.orthographicSize < zoomOutSize)
        {
            mainCamera.orthographicSize += zoomSpeed * Time.deltaTime;
            yield return null;
        }
        while (mainCamera.orthographicSize > zoomOutSize)
        {
            mainCamera.orthographicSize -= zoomSpeed * Time.deltaTime;
            yield return null;
        } 
    }
}
