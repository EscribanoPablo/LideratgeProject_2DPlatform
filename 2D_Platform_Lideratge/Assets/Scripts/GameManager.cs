using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager m_GameManager = null;
    public PlayerController m_Player;

    public Vector2 SpawnPosition { get; set; }

    //private void Awake()
    //{
    //    if (m_GameManager == null)
    //    {
    //        m_GameManager = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }   
    //}

    void Start()
    {
        Application.targetFrameRate = 60;
        UIManager.HideTelon();
    }

    public static GameManager GetGameManager()
    {
        if (m_GameManager == null)
        {
            GameObject l_gameObject = new GameObject("GameManager");
            m_GameManager = l_gameObject.AddComponent<GameManager>();
            GameManager.DontDestroyOnLoad(l_gameObject);
        }
        return m_GameManager;
    }

    public PlayerController GetPlayer()
    {
        return m_Player;
    }

}
