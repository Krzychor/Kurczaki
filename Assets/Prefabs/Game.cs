using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Player P1;
    public Player P2;
    public static Game game;
    public float WormFoodValue = 25f;
    public GameObject robak;
    [SerializeField]
    List<GameObject> Stages;

    [SerializeField]
    Material PrimaryPlayer1;
    [SerializeField]
    Material PrimaryPlayer2;
    [SerializeField]
    Material Secondary;

    Vector3 spawnpoint;
    float t, cd = 0;
    public float maxcd = 3f;
    public float Robak;
    GameObject bar1;
    GameObject bar2;
    float resetTime = 0;
    float ResetTimeThreshold = 5;
    public Text Message;

    void Awake()
    {
        bar1 = GameObject.Find("Bar1");
        bar2 = GameObject.Find("Bar2");
        game = this;
    }

    public void SpawnChicken(int stage, Player OldObject)
    {
        GameObject G;
        if (stage >= Stages.Count)
            return;
        if (stage < 0)
            return;
        Stages[stage].SetActive(false);
        int layer = 1 << 9;
     //   layer = layer;
        if (Physics.Raycast(OldObject.transform.position + new Vector3(0, 10, 0), Vector3.down, out RaycastHit hit, Mathf.Infinity, layer))
        {
            Debug.Log("better spawn, hit = " + hit.transform.name);
            G = Instantiate(Stages[stage], hit.point, OldObject.transform.rotation);
        }
        else
        {
            Debug.Log("normal spawn");
            G = Instantiate(Stages[stage], OldObject.transform.position, OldObject.transform.rotation);
        }


        Stages[stage].SetActive(true);
        Player p = G.GetComponent<Player>();
        p.stage = stage;
        p.SetNumber(OldObject.playerNumber);
        p.playerNumber = OldObject.playerNumber;
        G.SetActive(true);
    }



    IEnumerator reset()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Arena");
    }

    void Update()
    {
        if(P1.isDead && !P2.isDead)
        {
            Message.rectTransform.parent.gameObject.SetActive(true);
            Message.text = "Second Player Win!";
            StartCoroutine(reset());
        }
        if(!P1.isDead && P2.isDead)
        {
            Message.rectTransform.parent.gameObject.SetActive(true);
            Message.text = "First Player Win!";
            StartCoroutine(reset());
        }
        if (P1.isDead && P2.isDead)
            SceneManager.LoadScene("Arena");


        if (Input.GetKey(KeyCode.R))
            resetTime += Time.deltaTime;
        else
            resetTime = 0;
        if(resetTime > ResetTimeThreshold)
        {
            SceneManager.LoadScene("Arena");
        }
        summon(robak);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void summon(GameObject gameobject)
    {
        if (cd > maxcd)
        {
            spawnpoint = new Vector3(Random.Range(-49f, -40f), 46, Random.Range(-53.5f, -48f));
            Instantiate(gameobject, spawnpoint, Quaternion.identity);
            cd = 0;
        }

        cd += Time.fixedDeltaTime;
    }
}
