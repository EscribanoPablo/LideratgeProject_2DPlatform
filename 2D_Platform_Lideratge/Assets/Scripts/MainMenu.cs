using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    PlayerController m_Player;

    void Start()
    {
        m_Player = GameManager.GetGameManager().GetPlayer();
    }


    public void OnPlayButtonClicked()
    {
        if (m_Player != null)
        {
            m_Player.PlayAgain();
        }
        SceneManager.LoadScene("Prototype");
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
