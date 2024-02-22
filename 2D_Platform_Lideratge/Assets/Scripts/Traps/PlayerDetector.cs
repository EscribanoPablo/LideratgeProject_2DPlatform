using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerController m_Player;


    [Header("")]
    [SerializeField] bool m_CanDetect = true;
    public Action OnPlayerDetected;

    void Start()
    {
        m_Player = FindAnyObjectByType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!m_CanDetect) return;

        if (collision.transform == m_Player.transform)
        {
            Debug.Log("Player Detected");
            OnPlayerDetected?.Invoke();
        }
    }

   

    public void SetCanDetect(bool v)
    {
        m_CanDetect = v;
    }
    
}
