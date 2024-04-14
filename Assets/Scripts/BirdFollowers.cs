using System.Collections.Generic;
using UnityEngine;

public class BirdFollowers : MonoBehaviour
{
    [SerializeField] private int numFollowers;
    [SerializeField] private GameObject baseFollower;
    
    public float lerpTime = 0.5f;
    public static List<GameObject> followers = new List<GameObject>();
    //private bool doMoveFollowers = false;
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

    void FixedUpdate()
    {
        if (followers.Count > 0 && followers[^1].transform.position != transform.position)
        {
            followers[0].transform.position = Vector2.Lerp(followers[0].transform.position, transform.position, lerpTime);
        }
    }


    private void AddFollower()
    {
        followers.Add(Instantiate(baseFollower, transform.position, Quaternion.identity));
    }
}
