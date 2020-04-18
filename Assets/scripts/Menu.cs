using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    public Text status;
    public GameObject Loading;
    public GameObject menu;
    public string playerName = "unknowPlayer";
    public InputField nameField;


    string roomName_create = "";
    string roomName_load = "";

    public void UpdateName(InputField input)
    {
        if (input.text.Length > 0)
        {
            playerName = input.text.ToString();
            PlayerPrefs.SetString("playerName", playerName);
        }
        else
            input.text = playerName;

        PhotonNetwork.NickName = playerName;
    }

    public void UpdateRoomCreateName(InputField input)
    {
        roomName_create = input.text;
    }

    public void UpdateRoomLoadName(InputField input)
    {
        roomName_load = input.text;
    }



    void Start()
    {
        Loading.SetActive(true);
        menu.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PlayerPrefs.HasKey("playerName") == false)
            nameField.text = playerName;
        else
        {
            playerName = PlayerPrefs.GetString("playerName");
            nameField.text = playerName;
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }

        PhotonNetwork.NickName = playerName;
    }


    public void FastJoin()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRandomRoom();
        else
        {
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void Join()
    {
        if(roomName_load == "")
        {
            status.text = "set room name to join room";
            return;
        }
        PhotonNetwork.JoinRoom(roomName_load);
    }
    
    public void Create()
    {
        if(roomName_create == "")
        {
            status.text = "you can't create room with empty name!";
            return;
        }
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(roomName_create, options);
    }
    
    public override void OnConnectedToMaster()
    {
        Loading.SetActive(false);
        menu.SetActive(true);
  //      Debug.LogWarning("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
 //       Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        status.text = "room joing failed";
 //       Debug.LogWarning("room joing failed");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        //     PhotonNetwork.CreateRoom(null, new RoomOptions());
    }

    public override void OnJoinedRoom()
    {
     //   Debug.LogWarning("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }

    public override void OnCreatedRoom()
    {
   //     Debug.LogWarning("room created!");
        SceneManager.LoadScene("Lobby");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        status.text = "create room failed: " + message;
  //      Debug.LogWarning("create room failed: " + message);

    }
}
