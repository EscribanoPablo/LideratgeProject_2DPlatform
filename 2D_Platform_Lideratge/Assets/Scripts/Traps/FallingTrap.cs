using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerController m_Player;
    [SerializeField] FallingTrapTrigger m_fallingTrapTrigger;

    Rigidbody2D m_Rb;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_Rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnEnable()
    {
        m_fallingTrapTrigger.OnPlayerDetected += OnPlayerDetected;
    }

    private void OnDisable()
    {
        m_fallingTrapTrigger.OnPlayerDetected -= OnPlayerDetected;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Kill Player");
            StartCoroutine(m_Player.CODeath());
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.transform == m_Player.transform)
    //    {
    //        Debug.Log("Kill Player");
    //        StartCoroutine(m_Player.CODeath());
    //    }
    //}

    private void OnPlayerDetected()
    {
        m_Rb.bodyType = RigidbodyType2D.Dynamic;
        m_fallingTrapTrigger.SetCanDetect(false);
    }
}
