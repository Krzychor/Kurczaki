using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robak : MonoBehaviour
{
    Rigidbody rigid;
    float cd = 2.5f;
    readonly float maxcd = 2.5f;
    float lifetime = 0;
    readonly float maxlifetime = 20f;
    Vector3 velo;
   


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(cd>maxcd)
        {
            cd = 0;
            transform.RotateAround(Vector3.up, Random.Range(0, 360f));
        }

        if(lifetime>maxlifetime)
        {
            lifetime = 0;
            Destroy(gameObject);
        }
        lifetime += Time.fixedDeltaTime;

        cd += Time.fixedDeltaTime;

        velo = transform.right;
        velo.y = 0;

        rigid.velocity = new Vector3(velo.x, rigid.velocity.y, velo.z);
    }
}
