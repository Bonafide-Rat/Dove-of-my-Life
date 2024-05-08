using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pickupToSpawn;
    
    private GameObject spawnedPickup;

    private Vector3 spawnPos;

    [SerializeField] private float spawnTimeBetween;

    private float currentTimeLeft;

    private void Start()
    {
        currentTimeLeft = 0;
        spawnPos = transform.position + Vector3.right;
        spawnPos.z = 0;
    }

    // Start is called before the first frame update
    private void Update()
    {
        HandleSpawnPickup();
    }

    private void HandleSpawnPickup()
    {
        if (currentTimeLeft > 0)
        {
            currentTimeLeft -= Time.deltaTime;
        }
        
        if (spawnedPickup == null && currentTimeLeft <= 0)
        {
            spawnedPickup = Instantiate(pickupToSpawn, spawnPos, Quaternion.identity);
            spawnedPickup.GetComponent<FollowerPickup>().destroyOnPickup = true;
            currentTimeLeft = spawnTimeBetween;
        }
    }
    
}
