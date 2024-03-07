using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class EmotionSadness : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode _emotionInput;

    [Header("References")]
    //[SerializeField] private GameObject _umbrellaVFX;
    [SerializeField] private Animator _animator;

    [Header("Attributes")]
    [Range(0, 5)][SerializeField] private float _umbrellaGravity;
    [Range(0, 10)][SerializeField] private float _maxVerticalVelocity;
    [Range(0, 10)][SerializeField] private float _maxHorizontalVelocity;

    private Vector2 _maxVelocity;
    private Vector2 _initialMaxVelocity;

    private PlayerController _playerController;
    private float _initialGravity;
    private bool _isUmbrellaOpen;



    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }


    void Start()
    {
        _maxVelocity = new Vector2(_maxHorizontalVelocity, _maxVerticalVelocity);
        _initialMaxVelocity = new Vector3(_playerController.MaxHorizontalVelocity,
                                        _playerController.MaxVerticalVelocity);

        _initialGravity = _playerController.Gravity;
    }

    void Update()
    {      
        //if (Input.GetKeyDown(_emotionInput))
        //{
        //    if(_playerController.CanOpenUmbrella)
        //        OpenUmbrella();
        //}

        if (_isUmbrellaOpen == false) return;
        if (Input.GetKeyUp(_emotionInput) || _playerController.IsOnGround)
        {
            CloseUmbrella();
        }
    }

    public void OpenUmbrella()
    {
        _playerController.SetGravity(_umbrellaGravity);
        _playerController.StopVerticalVelocity();
        _playerController.SetMaxVelocity(_maxVelocity);
        _isUmbrellaOpen = true;
        _animator.SetBool("UmbrellaOpen", _isUmbrellaOpen);
        //_umbrellaVFX.SetActive(true);
        Debug.Log("Umbrella Open");
    }

    private void CloseUmbrella()
    {
        _isUmbrellaOpen = false;
        _playerController.SetGravity(_initialGravity);
        _playerController.SetMaxVelocity(_initialMaxVelocity);
        _animator.SetBool("UmbrellaOpen", _isUmbrellaOpen);
        //_umbrellaVFX.SetActive(false);
        Debug.Log("Umbrella Closed");
    }


}

