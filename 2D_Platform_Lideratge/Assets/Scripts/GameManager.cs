using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public Vector2 SpawnPosition { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        UIManager.HideTelon();
    }

    //public static GameManager GetGameManager()
    //{
    //    if (Instance == null)
    //    {
    //        GameObject l_gameObject = new GameObject("GameManager");
    //        Instance = l_gameObject.AddComponent<GameManager>();
    //        GameManager.DontDestroyOnLoad(l_gameObject);
    //    }
    //    return Instance;
    //}
    
}
