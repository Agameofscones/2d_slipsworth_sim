using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks {

    [Tooltip("Player prefab")]
    public GameObject playerPrefab;

    private void Start() {
        if (playerPrefab == null) {
            Debug.LogError("GameMananger is missing player prefab! Someone made a fucky wucky in the GameManager", this);
        }
        else {
            Debug.LogFormat("Attempting to instantiate local client");
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);//this is fucked, its 3D, aaaaaaaaaaaaaa
        }
        
    }
    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    private void LoadMap() {
        if (!PhotonNetwork.IsMasterClient) {
            Debug.LogError("PhotonNetwork: Tried to load level but we're not the master client!");
        }
        Debug.LogFormat("PhotonNetwork: Loading level with : {0} players connected", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("lobby room");//fuck you one lobby area, nothing else
    }
    //Photon Callbacks ahead
    public override void OnPlayerEnteredRoom(Player other) {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient) {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            LoadMap();
        }
    }
    public override void OnPlayerLeftRoom(Player other) {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
        if (PhotonNetwork.IsMasterClient) {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            LoadMap();//not sure if we want this, will force reloads on disconnect.
        }
    }
}