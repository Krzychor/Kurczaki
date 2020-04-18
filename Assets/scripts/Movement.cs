using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;


[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    MovementData data;

    Rigidbody body;
    Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
        
    }


    void Update()
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;
        data = GetComponentInChildren<MovementData>();
        body = GetComponent<Rigidbody>();

        NormalMovement();
    }


    void NormalMovement()
    {
        Vector3 velocity = new Vector3(0, 0, 0);
        float angle = 0;
        float rotSpeed = body.velocity.magnitude / data.ChickenSpeed;
        float speedRatio = body.velocity.magnitude / data.ChickenSpeed;
        if (speedRatio > 0)
            speedRatio = 0;
        rotSpeed = data.RotationSpeed * (1-speedRatio) + data.MovingRotationSpeed * speedRatio;
        
        if (Input.GetKey(KeyCode.D))
            angle += data.RotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            angle -= data.RotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            velocity += transform.forward * data.acceleration * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            velocity -= transform.forward * data.acceleration * Time.deltaTime;
        transform.Rotate(new Vector3(0, 1, 0), angle * rotSpeed * Time.deltaTime);
        body.velocity += velocity;

       
        
        Vector3 flat = body.velocity;
        flat.y = 0;
        float flatmag = flat.magnitude;
        if (flat.magnitude > data.ChickenSpeed)
        {
            flat.x /= flatmag;
            flat.z /= flatmag;
            body.velocity = new Vector3(flat.x*data.ChickenSpeed, body.velocity.y, flat.z*data.ChickenSpeed);
        }
        if (flat.magnitude < 0.1)
            body.velocity = new Vector3(0, body.velocity.y, 0);
        if (velocity.magnitude == 0)
            body.velocity -= new Vector3(flat.x / 5, 0, flat.z / 5);

        anim = GetComponentInChildren<Animator>();
        if (Input.GetKeyDown(KeyCode.Space) && flat.magnitude < 0.2)
        {
            body.velocity = new Vector3();
            anim.SetTrigger("eat");
        }

        if (body.velocity.magnitude > 0.01f)
        {
            anim.SetBool("isRunning", true);
        }
        else
            anim.SetBool("isRunning", false);
    }

    void FixedUpdate()
    {

    }
}
