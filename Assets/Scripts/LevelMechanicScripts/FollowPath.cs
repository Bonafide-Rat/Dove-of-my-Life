using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class FollowPath : MonoBehaviour
{

    // Array of waypoints to walk from one to the next one
    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]private Transform[] pauseWaypoints;
    private bool paused;

    // Walk speed that can be set in Inspector
    [SerializeField]
    private float moveSpeed = 2f;

    // Index of current waypoint from which Enemy walks
    // to the next one
    private int waypointIndex = 0;

    private SpriteRenderer spriteRenderer;
    public GameManagerScript gameManager;

    [HideInInspector]public bool doMoveChaser;

    private int resetIndex;

    // Use this for initialization
    private void Awake()
    {
        gameObject.SetActive(false); // Will be set to true once player enters its spawn region
        // Set position of Enemy as position of the first waypoint
        transform.position = waypoints[waypointIndex].transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        resetIndex = 0;
        // spriteRenderer.flipX = false;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(doMoveChaser);
        // Move Enemy
        if (doMoveChaser)
        {
            Move();
        }
        Flip();
    }

    // Method that actually make Enemy walk
    private void Move()
    {
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (waypointIndex < waypoints.Length - 1)
        {
            // Move Enemy from current waypoint to the next one
            // using MoveTowards method
            transform.position = 
                Vector2.MoveTowards(transform.position, waypoints[waypointIndex + 1].transform.position, moveSpeed * Time.deltaTime);

            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (Vector2.Distance(transform.position, waypoints[waypointIndex + 1].position) < 0.5f)
            {
                // Debug.Log("Pos reached.");
                waypointIndex += 1;
            }
        }

        if (pauseWaypoints.Any() && !paused)
        {
            foreach (var waypoint in pauseWaypoints)
            {
                if (waypoint.gameObject == waypoints[waypointIndex].gameObject)
                {
                    resetIndex = waypointIndex;
                    doMoveChaser = false;
                    paused = true;
                }
            }
        }
    }

    public void ResetToLastWaypoint()
    {
        transform.position = waypoints[waypointIndex].position; // Reset position to the last visited waypoint
    }

    public void ResetToInitialWaypoint(Vector2 playerPosition)
    {
        waypointIndex = resetIndex;
        if (paused)
        {
            transform.position = waypoints[waypointIndex].transform.position;
        }
        else
        {
            transform.position = new Vector2(playerPosition.x - 10, playerPosition.y);
        }
         // Position 10 units to the left of the player
        Debug.Log("Respawning enemy at last checkpoint");
    }

    private void Flip()
    {
        if (waypointIndex < waypoints.Length - 1)
        {
            // Determine if the target waypoint is to the left or right of the current position
            bool shouldFlip = (waypoints[waypointIndex + 1].position.x < transform.position.x && !spriteRenderer.flipX) ||
                              (waypoints[waypointIndex + 1].position.x > transform.position.x && spriteRenderer.flipX);
            if (shouldFlip)
            {
                // Flip the sprite by toggling the flipX property
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
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
}