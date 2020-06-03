using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks {
    #region private serializable fields
    string gameVersion = "1";//clients version number
    [Tooltip("Maximum connection count. Aka player count on a per room basis. If a client attempts to connect to a full room, a new one will be generated.")]
    [SerializeField]
    private byte maxPlayersPerRoom = 10;

    #endregion
    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    bool isConnecting;
    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;//forces PhotonNetwork.LoadLevel() on host server and all connected clients
    }
    private void Start() {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void Connect() {

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.JoinRandomRoom();
        }
        else {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            //PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    public override void OnConnectedToMaster() {
        if (isConnecting) {
            Debug.Log("On connected to master called by client launcher");
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
        
    }
    public override void OnDisconnected(DisconnectCause cause) {
        Debug.LogWarningFormat("Client Launcher called OnDisconnect with reason {0}", cause);
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        isConnecting = false;
    }
    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.LogWarning("Client launcher could not find a room, creating new room for client");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }
    public override void OnJoinedRoom() {
        Debug.Log("Client connected to room");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
            Debug.Log("Loading lobby level for initial connection");
            PhotonNetwork.LoadLevel("lobby room");//replace me with a global variable controlled by something.
        }
    }
}
