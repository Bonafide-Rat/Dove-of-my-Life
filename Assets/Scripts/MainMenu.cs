using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Level1(){
        SceneManager.LoadScene(1);
    }

    public void Level2(){
        SceneManager.LoadScene(2); //Change this to 2 or whatever buildindex it is after you add yours. Pref make the level = buildindex
    }

    public void Level3()
    {
        SceneManager.LoadScene(3);
    }

    public void Level4(){
        
    }

    public void ExitGame(){
        Application.Quit();
    }

}
