using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDetector : MonoBehaviour
{
    [Header("References")]
    protected PlayerController m_Player;


    void Start()
    {
        m_Player = GameManager.GetGameManager().GetPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //if (!m_CanDetect) return;
        if (m_Player != null)
        {
            if (collision.transform == m_Player.transform)
            {
                Debug.Log("Player Triggered");
                DoSomething();
            }
        }
        else
        {
            if (collision.tag == "Player")
            {
                Debug.Log("Player Triggered by Tag");
                DoSomething();
            }
        }

    }

    protected abstract void DoSomething();
   
}
