using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowBreak : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads the current scene
        }

        if (collision.gameObject.CompareTag("Follower"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}
