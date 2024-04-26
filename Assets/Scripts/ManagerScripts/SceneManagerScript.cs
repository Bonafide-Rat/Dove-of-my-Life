using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private int currSceneIndex;
    void Start()
    {
        currSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
    }


    private void ReloadScene()
    {
        PlayerController.isFacingRight = true;
        SceneManager.LoadScene(currSceneIndex);
    }
}
