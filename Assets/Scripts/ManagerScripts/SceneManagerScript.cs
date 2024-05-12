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
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void ReloadScene()
    {
        PlayerController.isFacingRight = true;
        SceneManager.LoadScene(currSceneIndex);
    }
}
