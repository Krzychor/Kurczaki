using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatZone : MonoBehaviour
{
    public Transform center;
    public List<GameObject> worms;
    public List<GameObject> players;

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

    public GameObject GetSmallestPlayer()
    {
        int stage = 99;
        GameObject choosen = null;
        foreach (GameObject G in players)
            if(G != null)
            {
                if(G.GetComponent<ChickenPlayer>().GetStage() < stage)
                {
                    choosen = G;
                    stage = G.GetComponent<ChickenPlayer>().GetStage();
                }
            }
        Debug.Log("return " + choosen);
        return choosen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "worm")
            if (!worms.Contains(other.gameObject))
                worms.Add(other.gameObject);
        if (other.gameObject.tag == "Player")
        {
            
            Transform par = other.transform;
            while (par.parent != null)
                par = par.parent;

            if (!players.Contains(par.gameObject))
                players.Add(par.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        worms.Remove(other.gameObject);
        Transform par = other.transform;
        while (par.parent != null)
            par = par.parent;
        players.Remove(par.gameObject);
    }
}
