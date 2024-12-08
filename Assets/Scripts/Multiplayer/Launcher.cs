using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Scripting;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to servers...");
        MenuManager.Instance.openMenu("loading");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Joining lobby...");
    }
    bool connectFirstTime = true;
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.openMenu("main");
        if (connectFirstTime)
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");

            connectFirstTime = false;
        }
        Debug.Log("Joined lobby!");
    }


    public InputField roomNameInputField;
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.openMenu("loading");
    }
    public Transform roomListContent;
    public GameObject roomListItem;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;

            Instantiate(roomListItem, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
        }
    }


    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.openMenu("loading");

    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.openMenu("loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.openMenu("main");
    }

    public Text roomNameText;
    public GameObject playerListItem;
    public Transform playerListContent;
    public GameObject startGameButton;
    
    public override void OnJoinedRoom()
    {
        MenuManager.Instance.openMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItem, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItem, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItem, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public Text errorText;
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        errorText.text = ("Failed to join room: " + message);
        MenuManager.Instance.openMenu("error");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = ("Room creation failed: " + message);
        MenuManager.Instance.openMenu("error");
    }



}
