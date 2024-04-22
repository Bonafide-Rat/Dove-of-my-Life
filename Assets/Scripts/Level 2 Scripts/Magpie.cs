using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MagpieController : MonoBehaviour
{
    public float speed = 1f; // Speed of movement
    public GameManagerScript gameManager;
    private bool snatched = false;
    public GameObject startPos;
    private bool sniffingPlayer;
    private bool startedMoving;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            StartMagpie();
        }
    }

    private void Update()
    {
        if (startedMoving)
        {
            if (!snatched && !sniffingPlayer)
            {
                // Calculate the movement vector
                Vector3 movement = Vector3.right * speed * Time.deltaTime;

                // Update the position of the GameObject
                transform.position += movement;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    { 
        if (!snatched && collider.CompareTag("Player") && !GameManagerScript.playerInCover)
        {
            snatched = true;
            gameManager.gameOver();
        }
        
        else if (!snatched && collider.CompareTag("Player") && GameManagerScript.playerInCover || collider.CompareTag("MagpieBoundary"))
        {
            Debug.Log(startPos.transform.position);
            sniffingPlayer = true;
            StartCoroutine(SniffPlayer());
        }
    }

    IEnumerator SniffPlayer()
    {
        yield return new WaitForSeconds(3);
        transform.position = startPos.transform.position;
        yield return new WaitForSeconds(5);
        sniffingPlayer = false;
    }

    public void StartMagpie()
    {
        startedMoving = true;
    }
}