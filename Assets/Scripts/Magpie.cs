using UnityEngine;

public class MagpieController : MonoBehaviour
{
    public float speed = 1f; // Speed of movement
    public GameManagerScript gameManager;
    private bool snatched = false;

    private void Start()
    {

    }

    private void Update()
    {
        if (!snatched)
        {
            // Calculate the movement vector
            Vector3 movement = Vector3.right * speed * Time.deltaTime;

            // Update the position of the GameObject
            transform.position += movement;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    { 
        if (!snatched && collider.CompareTag("Player") || !snatched && collider.CompareTag("Ring"))
        {
            snatched = true;
            gameManager.gameOver();
        }
    }
}