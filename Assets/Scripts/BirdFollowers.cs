using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFollowers : MonoBehaviour
{
    [SerializeField] private int numFollowers;
    [SerializeField] private GameObject baseFollower;
    
    public float lerpTime = 0.5f;
    public static List<GameObject> followers = new List<GameObject>();
    //private bool doMoveFollowers = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numFollowers; i++)
        {
            AddFollower();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            AddFollower();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followers.Count > 0)
        {
            if (followers[followers.Count -1].transform.position != transform.position)
            {
                followers[0].transform.position = Vector2.Lerp(followers[0].transform.position,transform.position,lerpTime);
            }
        }
    }


    public void AddFollower()
    {
        followers.Add(Instantiate(baseFollower, transform.position, Quaternion.identity));
        Debug.Log("added" + followers[followers.Count -1]);
    }
}
