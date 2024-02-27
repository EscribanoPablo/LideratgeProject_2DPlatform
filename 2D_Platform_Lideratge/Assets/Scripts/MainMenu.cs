using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        
    }


    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Prototype");
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}