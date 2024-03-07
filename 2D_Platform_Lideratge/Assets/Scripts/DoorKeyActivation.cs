using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyActivation : PlayerDetector
{
    [SerializeField] Animator m_DoorAnimator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        gameObject.SetActive(true);
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
    }
}
