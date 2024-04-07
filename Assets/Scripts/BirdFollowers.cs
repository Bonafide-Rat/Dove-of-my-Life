using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFollowers : MonoBehaviour
{
    [SerializeField] private int numFollowers;
    [SerializeField] private GameObject baseFollower;
    
    public float lerpTime = 0.5f;
    public List<GameObject> followers = new List<GameObject>();
    private bool doMoveFollowers = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numFollowers; i++)
        {
            followers.Add(Instantiate(baseFollower, transform.position, Quaternion.identity));
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (followers[followers.Count -1].transform.position != transform.position)
        {
            followers[0].transform.position = Vector2.Lerp(followers[0].transform.position,transform.position ,lerpTime);
        }
    }
}
