using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnFinishedEatingFunction : MonoBehaviour
{
    public void OnFinishedEating()
    {
        GameObject G = GetComponentInChildren<EatZone>().GetWorm();
        if (G == null)
            return;

        PhotonView.Get(this).RPC("KillWorm", RpcTarget.MasterClient, G.GetComponentInChildren<PhotonView>().ViewID);
        GameMaster.game.health += GameMaster.game.FoodGain;
    }


    [PunRPC]
    void KillWorm(int id)
    {
        PhotonNetwork.Destroy(PhotonView.Find(id).gameObject);
    }

}
