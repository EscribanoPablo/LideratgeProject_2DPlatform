using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorKeyActivation : PlayerDetector
{
    [SerializeField] Animator m_DoorAnimator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    AudioSource audioSource;
    AudioSource audioSource2;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        gameObject.SetActive(true);
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        
    }
    protected override void DoSomething()
    {
        OpenDoor();

    }

    private void OpenDoor()
    {
        m_DoorAnimator.SetTrigger("KeyPicked");
        m_DoorAnimator.SetBool("KeyPicked", true);

        gameObject.SetActive(false); // or disable spriteRender
        //spriteRenderer.enabled = false;
        //boxCollider.enabled = false;
        SoundManager.PlaySFX("COLLECT", audioSource);
        SoundManager.PlaySFX("DOOROPEN", audioSource2);
    }
}
