using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowersScript : MonoBehaviour
{
    private List<GameObject> followers;
    private FollowerManager mainScript;
    private int myIndex;
    private float jitterMagnitude = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GetComponent<Collider2D>().enabled = false;
        mainScript = GameObject.FindWithTag("Player").GetComponent<FollowerManager>();
        followers = FollowerManager.followers;
        myIndex = followers.IndexOf(gameObject);
    }

    private void Update()
    {
        myIndex = followers.IndexOf(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("TriggerZone"))
        {
            Debug.Log(other.gameObject.name);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("TriggerZone"))
        {
            Debug.Log(other.gameObject.name);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followers.Count <= 0) return;
        if (myIndex > 0)
        {
            transform.position = Vector2.Lerp(transform.position,followers[myIndex - 1].transform.position + new Vector3(Random.Range(-jitterMagnitude, jitterMagnitude),Random.Range(-jitterMagnitude, jitterMagnitude) ),mainScript.lerpTime);
        }
    }
}
