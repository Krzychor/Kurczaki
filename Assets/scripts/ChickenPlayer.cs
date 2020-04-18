using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChickenPlayer : MonoBehaviour
{
    int stage = 0;
    public GameObject model;
    
    

    void Devolve()
    {
        stage--;
        if (stage < 0)
        {
            stage++;
            return;
        }
        GameMaster.game.health = GameMaster.game.maxHealth / 2;
        if (model != null)
            PhotonNetwork.Destroy(model);
        GameObject G = PhotonNetwork.Instantiate("prefabs/" + GameMaster.game.stages[stage].name, new Vector3(0, 0, 0), Quaternion.identity);
        G.transform.SetParent(transform, false);
        model = G;

        Rigidbody body = GetComponent<Rigidbody>();
    }

    
    void Evolve()
    {
        stage++;
        if (stage > GameMaster.game.stages.Count)
        {
            stage--;
            return;
        }


        if (model != null)
            PhotonNetwork.Destroy(model);

        GameObject G = PhotonNetwork.Instantiate("prefabs/"+GameMaster.game.stages[stage].name, new Vector3(), GameMaster.game.stages[stage].transform.rotation);
        G.transform.SetParent(transform, false);
        G.AddComponent<OnFinishedEatingFunction>();
        model = G;

        Rigidbody body = GetComponent<Rigidbody>();
        return;
        //      transform.position += new Vector3(0, 30, 0);
        //if (body.SweepTest(new Vector3(0, -1, 0), out RaycastHit hit))
        //{
        //    Collider coll = GetComponentInChildren<Collider>();

        //    transform.position = hit.point + new Vector3(0, -coll.bounds.min.y, 0);
        //    G.transform.position = new Vector3();
        //    //transform.position = hit.point;
        //}
    }


    [PunRPC]
    void Win(string name)
    {
        GameMaster.game.SetWinner(name);
    }



    void Update()
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.R))
            GameMaster.game.health = GameMaster.game.maxHealth;


        if (GameMaster.game.health < 0 && stage > 1)
            Devolve();
        else if (GameMaster.game.health >= GameMaster.game.maxHealth)
        {
            if (stage + 1 == GameMaster.game.stages.Count)
                return;
            GameMaster.game.health = GameMaster.game.maxHealth / 2;
            PhotonView photonView = PhotonView.Get(this);
            Evolve();
//            photonView.RPC("Evolve", RpcTarget.MasterClient);
        }
    }
}
