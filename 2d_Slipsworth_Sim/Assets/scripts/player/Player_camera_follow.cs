using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Player_camera_follow : MonoBehaviourPunCallbacks
{
    public GameObject playerInstance;
    private Transform playerTransform;
    bool isAttached = false;
    bool isAlive = false;

    private void Start() {
        playerTransform = playerInstance.GetComponent<Transform>();

        if (playerInstance.GetComponent<Player_camera_follow>() != null) {
            isAttached = true;
        }
        else {
            Debug.LogError("Player Instance didn't have a camera TO follow");
            return;//early return if no camera
        }
    }
    public void setAlive(bool alive) {
        if (alive == true) {
            isAlive = true;
        }
        else if (alive == false)
            isAlive = false;
    }
    private void FixedUpdate() {
        if (isAlive == true) {
            FollowPlayer();
        }
    }
    private void FollowPlayer() {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}
