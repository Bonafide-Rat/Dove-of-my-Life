using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFollowers : MonoBehaviour
{
    [SerializeField] private int numFollowers;
    [SerializeField] private GameObject baseFollower;

    public float savePosTime = 0.2f;
    public float lerpTime = 0.5f;
    private Rigidbody2D birdBodyRB;
    private List<GameObject> followers = new List<GameObject>();

    private List<Vector2> playerPosList = new List<Vector2>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numFollowers; i++)
        {
            followers.Add(Instantiate(baseFollower, transform.position, Quaternion.identity));
        }
        birdBodyRB = GetComponent<Rigidbody2D>();
        
        InvokeRepeating(nameof(addPlayerPos), 0, savePosTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPosList.Count > 0)
        {
            
        }
      
        
        if (followers[followers.Count - 1].transform.position == transform.position)
        {
            playerPosList.Clear();
        }

        else
        {
            foreach (var follower in followers)
            {
                for (int i = 0; i < playerPosList.Count; i++)
                {
                    follower.transform.position = Vector2.Lerp(follower.transform.position, playerPosList[i],lerpTime + (followers.IndexOf(follower)/100 ));
                }
            }
        }
    }

    private void addPlayerPos()
    {
        playerPosList.Add(transform.position);
    }

    
}
