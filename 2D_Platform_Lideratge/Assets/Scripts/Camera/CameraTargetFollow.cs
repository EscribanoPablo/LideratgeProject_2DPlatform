using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    
    [SerializeField] PlayerController _player;

    // Start is called before the first frame update
    void Start()    
    {
        RoomCamManager roomCamManager = RoomCamManager.GetCameraManager();
        roomCamManager.AddToCamList(_virtualCamera);
        roomCamManager.SetCurrentRoomCam(_virtualCamera);
        _virtualCamera.Follow = _player.transform;
        _virtualCamera.LookAt = _player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
