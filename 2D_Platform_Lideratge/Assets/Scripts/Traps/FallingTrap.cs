using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    [Header("References")]
    PlayerController m_Player;
    [SerializeField] FallingTrapTrigger m_fallingTrapTrigger;

    Rigidbody2D m_Rb;
    [SerializeField] float minVelocityToKill = 3;
    bool canKill;

    private void Awake()
    {
        gameObject.AddComponent<AudioSource>().playOnAwake = false;
    }
    private void Start()
    {
        m_Player = GameManager.GetGameManager().GetPlayer();
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
        if (collision.transform == m_Player.transform)
        {
            if (canKill)
            {
                Debug.Log("Kill Player");
                StartCoroutine(m_Player.CODeath());
            }
        }
        else if(collision.collider.tag != "Wall")
        {
            Debug.Log("Hit Ground");
            SoundManager.PlaySFX(AudioNames.BLOQUE, GetComponent<AudioSource>());
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

    private void Update()
    {
        if (m_Rb.bodyType == RigidbodyType2D.Static) return;
        if (m_Rb.velocity.y < minVelocityToKill && m_Rb.velocity.y > -minVelocityToKill)
        {
            canKill = false;
        }
        else { canKill = true; }
    }

    private void OnPlayerDetected()
    {
        m_Rb.bodyType = RigidbodyType2D.Dynamic;
        m_fallingTrapTrigger.SetCanDetect(false);
    }
}
