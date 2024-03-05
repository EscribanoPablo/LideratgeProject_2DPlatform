using System.Collections;
using UnityEngine;



public class BreakablePlatform : MonoBehaviour
{

    [SerializeField] private float _timeBeforeBreak;
    [SerializeField] private float _timeForReapear;

    [SerializeField] private ParticleSystem _breakParticles;

    private SpriteRenderer _spriteRenderer;
    private Collider2D[] _colliders;
    private bool _isBreaking;


    private WaitForSeconds _beforeBreakWait;
    private WaitForSeconds _ReapearWait;


    private void Awake()
    {
        _beforeBreakWait = new WaitForSeconds(_timeBeforeBreak);
        _ReapearWait = new WaitForSeconds(_timeForReapear);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _colliders = GetComponents<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(!_isBreaking)
                StartCoroutine(CO_BreakPlatform());
        }
    }

    IEnumerator CO_BreakPlatform()
    {
        _isBreaking = true;
        yield return _beforeBreakWait;

        _spriteRenderer.enabled = false;
        foreach(Collider2D collider in _colliders) { collider.enabled = false; }
        _breakParticles.Play();

        yield return _ReapearWait;

        _spriteRenderer.enabled = true;
        foreach (Collider2D collider in _colliders) { collider.enabled = true; }
        _isBreaking = false;

    }


}
