using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class network_PlayerManager : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;
    void Start() {
        Player_camera_follow _playerCam = this.gameObject.GetComponent<Player_camera_follow>();

        if (_playerCam != null) {
            if (photonView.IsMine) {
                _playerCam.setAlive(true);
            }
            else {
                Debug.LogError("Network_playerManager couldn't set the local client's camera because we're not the local client (???). Local camera updates killed.", this);
                _playerCam.setAlive(false);//wait I think this also kills all other clients camera controls. Fuck.
            }
        }
        else {
            Debug.LogError("Network_playerManager couldn't find the local client's Player_camera_follow script component.", this);
        }
    }
    void Awake() {
        if (photonView.IsMine) {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
       
    }
}
