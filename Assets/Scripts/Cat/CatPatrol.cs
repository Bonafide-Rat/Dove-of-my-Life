using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CatPatrol : MonoBehaviour
{
    public GameManagerScript gameManager;

    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    [SerializeField] public float speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move towards the current target point.
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, speed * Time.fixedDeltaTime);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            flip();
            // Toggle between point A and point B
            currentPoint = (currentPoint == pointB.transform) ? pointA.transform : pointB.transform;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !GameManagerScript.playerInCover) // Assuming the player has a tag of "Player"
        {
            //gameManager.gameOver();
            gameManager.respawn();
        }
    }


    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
