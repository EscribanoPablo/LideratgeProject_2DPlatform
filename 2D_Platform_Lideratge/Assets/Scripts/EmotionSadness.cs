using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class EmotionSadness : MonoBehaviour
{
    [SerializeField] private KeyCode _emotionInput;

    [SerializeField] private float _umbrellaGravity;
    [SerializeField] private float _maxVerticalVelocity;

    private PlayerController _playerController;
    private float _initialGravity;
    private float _initialMaxVerticalVelocity;
    private bool _isUmbrellaOpen;



    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }


    void Start()
    {
        _initialGravity = _playerController.Gravity;
        _initialMaxVerticalVelocity = _playerController.MaxVerticalVelocity;
    }

    void Update()
    {      
        if (Input.GetKeyDown(_emotionInput))
        {
            OpenUmbrella();
        }

        if (_isUmbrellaOpen == false) return;
        if (Input.GetKeyUp(_emotionInput) || _playerController.IsOnGround)
        {
            CloseUmbrella();
        }
    }

    private void OpenUmbrella()
    {
        _playerController.SetGravity(_umbrellaGravity);
        _playerController.StopVerticalVelocity();
        _playerController.SetMaxVerticalVelocity(_maxVerticalVelocity);
        _isUmbrellaOpen = true;
        Debug.Log("Umbrella Open");
    }

    private void CloseUmbrella()
    {
        _isUmbrellaOpen = false;
        _playerController.SetGravity(_initialGravity);
        _playerController.SetMaxVerticalVelocity(_initialMaxVerticalVelocity);
        Debug.Log("Umbrella Closed");

    }
}

