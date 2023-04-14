using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    [Header("Room Controls")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private string roomNameToCreate;
    
    [Header("Debug Texts")]
    [SerializeField] private TextMeshProUGUI serverDebugTextUI;
    [SerializeField] private TextMeshProUGUI isConnectedToRoomDebugTextUI;
    [SerializeField] private TextMeshProUGUI currentRoomNameDebugTextUI;
    [SerializeField] private TextMeshProUGUI currentRoomPlayerCountDebugTextUI;
    
    public void LoginToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("<color=#00ff00>We are connected!</color>");
        createRoomButton.interactable = true;
    }

    public void CreateRoom()
    {
        createRoomButton.interactable = false;
        PhotonNetwork.JoinOrCreateRoom(roomNameToCreate, new RoomOptions(), null);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("We are in a room!");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        isConnectedToRoomDebugTextUI.text = "Yes!";
        RefreshCurrentRoomUI();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        RefreshCurrentRoomUI();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError("Create failed..." + Environment.NewLine + message);
        createRoomButton.interactable = true;
    }

    private void Start()
    {
        isConnectedToRoomDebugTextUI.text = "No";
        createRoomButton.interactable = false;
        currentRoomNameDebugTextUI.text = string.Empty;
    }

    private void Update()
    {
        serverDebugTextUI.text = PhotonNetwork.NetworkClientState.ToString();
    }

    private void RefreshCurrentRoomUI()
    {
        currentRoomNameDebugTextUI.text = PhotonNetwork.CurrentRoom.Name;
        currentRoomPlayerCountDebugTextUI.text = string.Format("{0}/{1}", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
    }
}
