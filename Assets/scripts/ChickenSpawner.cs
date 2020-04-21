using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChickenSpawner : MonoBehaviour
{
    public GameObject SpawnFrom;
    public GameObject SpawnTo;
    public GameObject ChickenBotPrefab;
    [Range(0.1f, 100)]
    public float MinSpawnTime = 5;
    [Range(0.1f, 100)]
    public float MaxSpawnTime = 15;

    public float FromLastSpawn = 0;

    private void OnValidate()
    {
        if (MaxSpawnTime < MinSpawnTime)
            MaxSpawnTime = MinSpawnTime;
    }
    
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if(GameMaster.game.CurrentChickens < GameMaster.game.ChickenBotsLimit)
        {
            if (FromLastSpawn < 0)
            {
                FromLastSpawn = Random.Range(MinSpawnTime, MaxSpawnTime);
                Vector3 pos;
                Vector3 min = new Vector3();
                Vector3 max = new Vector3();
                if (SpawnFrom.transform.position.x < SpawnTo.transform.position.x)
                { min.x = SpawnFrom.transform.position.x; max.x = SpawnTo.transform.position.x; }
                else
                { min.x = SpawnTo.transform.position.x; max.x = SpawnFrom.transform.position.x;  }
                if (SpawnFrom.transform.position.z < SpawnTo.transform.position.z)
                { min.z = SpawnFrom.transform.position.z; max.z = SpawnTo.transform.position.z;  }
                else
                {  min.z = SpawnTo.transform.position.z; max.z = SpawnFrom.transform.position.z; }
                pos = new Vector3(Random.Range(min.x, max.x), 99, Random.Range(min.z, max.z));


                if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                {
                    pos.y = hit.point.y;
                }
                else
                    return;
                PhotonNetwork.Instantiate("prefabs/" + ChickenBotPrefab.name, pos, Quaternion.identity);
                GameMaster.game.CurrentChickens++;
            }
            else
                FromLastSpawn -= Time.deltaTime;
        }
    }
}
