using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{
    public GameObject LaunchButton;
    public List<GameObject> Slots; //set in inspector

    public void Launch()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        else
            SceneManager.LoadScene("testScene");
    }

    public void Exit()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            LaunchButton.SetActive(false);
        else
            LaunchButton.SetActive(true);
    }

    private void Start()
    {
        RefreshPlayers();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }
    
    public override void OnPlayerLeftRoom(Player other)
    {
        RefreshPlayers();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
    //    Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        RefreshPlayers();
    }

    public void RefreshPlayers()
    {
        int slotIndex = 0;
        Dictionary<int, Photon.Realtime.Player> pList = PhotonNetwork.CurrentRoom.Players;
        foreach (KeyValuePair<int, Photon.Realtime.Player> p in pList)
        {
            Slots[slotIndex].GetComponentInChildren<Text>().text = p.Value.NickName;
            Slots[slotIndex].SetActive(true);
            slotIndex++;
        }

        while(slotIndex < Slots.Count)
        {
            Slots[slotIndex].SetActive(false);
            slotIndex++;
        }
    }
}
