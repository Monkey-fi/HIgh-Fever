using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene(1);
    }  
    public void back()
    {
        SceneManager.LoadScene(0);
    }
    public void settings()
    {
        SceneManager.LoadScene(2);
    }
    public void Quit()
    {
        Application.Quit();
    }

}
