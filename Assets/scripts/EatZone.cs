using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatZone : MonoBehaviour
{
    public Transform center;
    public List<GameObject> worms;

    Vector3 point;





    private void Awake()
    {
        if (center == null)
            point = transform.position;
        else
            point = center.position;
    }


    public GameObject GetWorm()
    {
        GameObject choosen = null;
        float d;
        float min = float.MaxValue;
        foreach(GameObject G in worms)
        {
            if(G != null)
            {
                d = (G.transform.position - point).magnitude;
                if (d < min)
                {
                    choosen = G;
                    min = d;
                }
            }
        }
        return choosen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "worm")
            if (!worms.Contains(other.gameObject))
                worms.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        worms.Remove(other.gameObject);
    }
}
