using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerController m_Player;


    void Start()
    {
        m_Player = FindAnyObjectByType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //if (!m_CanDetect) return;

        if (collision.transform == m_Player.transform)
        {
            Debug.Log("Player Triggered");
            DoSomething();
        }
    }

    protected abstract void DoSomething();
   
}
