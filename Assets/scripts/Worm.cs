using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Worm : MonoBehaviour
{
    Rigidbody rigid;
    float cd = 2.5f;
    readonly float maxcd = 2.5f;
    float lifetime = 0;
    Vector3 velo;


    private void OnDestroy()
    {
        GameMaster.game.currentWorms--;
    }


    void Start()
    {
        GameMaster.game.currentWorms++;
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(cd>maxcd)
        {
            cd = 0;
            transform.Rotate(Vector3.up, Random.Range(0, 360f));
        }

        if(lifetime>GameMaster.game.WormMaxlifetime)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                lifetime = 0;
                PhotonNetwork.Destroy(gameObject);
                return;
            }
        }
        else
            lifetime += Time.fixedDeltaTime;

        cd += Time.fixedDeltaTime;

        velo = transform.right;
        velo.y = 0;
        rigid.velocity = new Vector3(velo.x, rigid.velocity.y, velo.z);
    }
}
