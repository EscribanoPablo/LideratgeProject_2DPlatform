using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager m_GameManager = null;
    public PlayerController m_Player;

    public Vector2 SpawnPosition { get; set; }

    private List<FallingTrap> fallingTraps;
    private List<DoorKeyActivation> keys;

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
            m_GameManager.fallingTraps = new List<FallingTrap>();
            m_GameManager.keys = new List<DoorKeyActivation>();
            GameManager.DontDestroyOnLoad(l_gameObject);
        }
        return m_GameManager;
    }

    public PlayerController GetPlayer()
    {
        return m_Player;
    }

    public void Restart()
    {
        m_Player.Restart();

        foreach (FallingTrap trap in fallingTraps)
        {
            trap.Restart();
        }

        foreach (DoorKeyActivation key in keys)
        {
            key.Restart();
        }
    }

    public void AddFallingTrap(FallingTrap fallingTrap)
    {
        fallingTraps.Add(fallingTrap);
    }

    public void AddKey(DoorKeyActivation key)
    {
        keys.Add(key);
    }

}
