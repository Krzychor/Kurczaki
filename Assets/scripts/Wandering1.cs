using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Wandering1 : MonoBehaviour
{
    [Range(0, 89)]
    public float angle = 45;
    [Min(0)]
    public float speed = 15;




    // Start is called before the first frame update
    void Start()
    {
        SetRandomVelocity();
    }

    void SetRandomVelocity()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        Vector3 dir = new Vector3(Random.Range(0, 1), 0, Random.Range(0, 1));
        rigid.velocity = dir * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        Rigidbody rigid = GetComponent<Rigidbody>();
        Vector3 d = rigid.velocity;
        if (d.magnitude != 0)
        {
            d /= d.magnitude;
            d *= speed;
            rigid.velocity = d;
        }
        else
            SetRandomVelocity();

        if(transform.position.y < -10)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        if (!isGround(collision))
        {
            Vector3 flat = -rigid.velocity;
            flat.y = 0;
            rigid.velocity = new Vector3(flat.x, rigid.velocity.y, flat.z);
            transform.LookAt(transform.position + flat);

        }
    }

    bool isGround(Collision collision)
    {
      //  float minDot = GetMinDot(collision.gameObject.layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= angle)
            {
                //     groundContactCount += 1;
                //       contactNormal += normal;
                return true;
            }
            else if (normal.y > -0.01f)
            {
          //      steepContactCount += 1;
         //       steepNormal += normal;
            }
        }
        return false;
    }
}
