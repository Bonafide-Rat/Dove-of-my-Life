using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowersScript : MonoBehaviour
{
    private List<GameObject> followers;
    [SerializeField] private FollowerManager mainScript;
    private int myIndex;
    private float jitterMagnitude = 1f;


    private void Awake()
    {
        if (GameObject.FindWithTag("Player").GetComponent<FollowerManager>() != null)
        {
            mainScript = GameObject.FindWithTag("Player").GetComponent<FollowerManager>();
        }
    }

    void Start()
    {
        //GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<Collider2D>().enabled = false;
        
        followers = FollowerManager.followers;
        myIndex = followers.IndexOf(gameObject);
        if (mainScript == null)
        {
            Debug.LogError("FollowerScript: MainScript is null. Check the Player tag or component setup.");
        }
        if (followers == null)
        {
            Debug.LogError("FollowerScript: Followers list is null. Check FollowerManager initialization.");
        }
    }

    private void Update()
    {
        myIndex = followers.IndexOf(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("TriggerZone") || !other.CompareTag("WindZone"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("TriggerZone") || !other.gameObject.CompareTag("WindZone"))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followers.Count <= 0) return;
        if (myIndex > 0 && followers != null && followers.Count > myIndex - 1 && followers[myIndex - 1] != null)
        {
            transform.position = Vector2.Lerp(transform.position,followers[myIndex - 1].transform.position + new Vector3(Random.Range(-jitterMagnitude, jitterMagnitude),Random.Range(-jitterMagnitude, jitterMagnitude) ),mainScript.lerpTime);
        }
    }
}
