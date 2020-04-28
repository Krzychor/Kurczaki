using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameMaster : MonoBehaviour
{
    public static GameMaster game;

    public new CameraFollow camera;
    public Transform SpawnPoint;
    public GameObject wormPrefab;
    public GameObject Player;
    public List<GameObject> stages;


    [Range(0.01f, 5)]
    public float healtDepletionPerSec = 1;
    public float health = 0;
    public int maxHealth = 200;
    [Range(0, 100)]
    public int FoodGain = 20;

    [Range(2, 100)]
    public float WormMaxlifetime = 20f;
    [Range(0, 100)]
    public float spawnRange = 10;
    float TimeToNextWorm = 0;
    [Range(0.5f, 10)]
    public float minWormSpawnTime = 0.5f;
    [Range(0, 10)]
    public float maxWormSpawnTime = 2;
    public uint currentWorms = 0;
    [Range(0, 100)]
    public uint maxWorms = 20;

    [Min(0)]
    public int ChickenBotsLimit = 100;
    public int CurrentChickens = 0;


    public void SetWinner(string name)
    {

    }

    void Awake()
    {
        game = this;
        health = maxHealth / 2;
    }

    void Start()
    {
        GameObject player = PhotonNetwork.Instantiate("prefabs/Player", SpawnPoint.position, Quaternion.identity, 0);

        camera.target = player.transform;
        camera.enabled = true;
        GameObject G = PhotonNetwork.Instantiate("prefabs/stage 1", SpawnPoint.position, Quaternion.identity, 0);
        G.transform.SetParent(player.transform);
        G.AddComponent<OnFinishedEatingFunction>();
        camera.target.GetComponent<ChickenPlayer>().model = G;


        if (player.GetComponent<Rigidbody>().SweepTest(new Vector3(0, -1, 0), out RaycastHit hit))
        {
            Collider coll = player.GetComponentInChildren<Collider>();
            
            player.transform.position = hit.point + new Vector3(0, -coll.bounds.min.y, 0);
            //transform.position = hit.point;
        }
    }


    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            health -= healtDepletionPerSec * Time.deltaTime;

            TimeToNextWorm -= Time.deltaTime;
         //   if (currentWorms < maxWorms)
            {
                if (TimeToNextWorm <= 0)
                {
                    Vector3 point = new Vector3(Random.Range(0, spawnRange), 46, Random.Range(0, spawnRange));
                    PhotonNetwork.Instantiate("prefabs/worm", SpawnPoint.position + point, Quaternion.identity);
                    TimeToNextWorm = Random.Range(minWormSpawnTime, maxWormSpawnTime);
                }
            }
        }


    }
}
