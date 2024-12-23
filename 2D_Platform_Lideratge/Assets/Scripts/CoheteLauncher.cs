using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class CoheteLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _cohete;
    [SerializeField] private Transform _shootPosition;

    [SerializeField] private Vector2 _coheteSpeed;
    [SerializeField] private float _shootTime;
    [SerializeField] private float _startDelay;
    [SerializeField] private float timeToDestroy;

    [SerializeField] private ParticleSystem _ShootParticles;
    private AudioSource _audioSource;

    private float _shootTimer;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        _shootTimer = Time.time + _shootTime + _startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= _shootTimer)
        {
            _shootTimer = Time.time + _shootTime;
            Shoot();
        }
    }

    private void Shoot()
    {
        SoundManager.PlaySFX(AudioNames.SHOOT, _audioSource);
        Rigidbody2D rb = Instantiate(_cohete, _shootPosition.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        rb.velocity = _coheteSpeed;
        Destroy(rb.gameObject, timeToDestroy);
        _ShootParticles.Play();
    }
}
