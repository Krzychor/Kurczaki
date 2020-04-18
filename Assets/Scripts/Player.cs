using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  //  public GameObject player;
    [SerializeField]
    GameObject bar;
    [SerializeField]
    BoxCollider EatZone;
    //  public float startScale = 1;
    public string playerNumber = "1";
    public int stage = 1; //current stage, set at spawn
    
    private float inputHorizontal;
    private float inputVertical;

    new Rigidbody rigidbody;

    Bar starveBar;
    Animator playerAnim;

    string axisVertical;
    string axisHorizontal;

    public bool isDead = false;

    IEnumerator eat(GameObject G)
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(G);
    }

    public void TryEat() // after clicking controll
    {
        Collider[] results = new Collider[10];
        Physics.OverlapBoxNonAlloc(transform.position, EatZone.bounds.extents, results);
        for(int i = 0; i < results.Length; i++)
        {
            if (results[i] != null && ((results[i].tag == "Robak" && stage < 3) || (results[i].tag == "Player" && stage == 3)))
            {
                playerAnim.SetTrigger("eat");
                results[i].tag = "Untagged";
                Destroy(results[i].gameObject);

                if (results[i].GetComponent<Player>() != null)
                    results[i].GetComponent<Player>().isDead = true;
                starveBar.AddBar(Game.game.WormFoodValue);

                if (starveBar.isFull())
                    LevelUp();
            }
        }
    }


    private void OnValidate()
    {
        axisVertical = "Vertical" + playerNumber;
        axisHorizontal = "Horizontal" + playerNumber;
    }

    public void SetNumber(string number)
    {
        playerNumber = number;
        axisVertical = "Vertical" + playerNumber;
        axisHorizontal = "Horizontal" + playerNumber;
        bar = GameObject.Find("Bar" + playerNumber);
        starveBar = bar.GetComponent<Bar>();
    }
    
    void LevelUp()
    {
        starveBar.SetBar(starveBar.max / 2);
        Game.game.SpawnChicken(stage + 1, this);
        Destroy(gameObject);
    }

    void LevelDown()
    {
        if(stage > 1)
        {
            Game.game.SpawnChicken(stage - 1, this);
            Destroy(gameObject);
            starveBar.SetBar(starveBar.max / 2);
        }        
    }

    void Start()
    {
        EatZone = GetComponentInChildren<BoxCollider>();
        rigidbody = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        bar = GameObject.Find("Bar" + playerNumber);
        starveBar = bar.GetComponent<Bar>();
        starveBar.SetBar(starveBar.max / 2f);
        axisVertical = "Vertical" + playerNumber;
        axisHorizontal = "Horizontal" + playerNumber;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            starveBar.setFull();

        if (starveBar.isFull())
            LevelUp();

        if (starveBar.GetBar() <= 0)
            LevelDown();

        if (Input.GetButtonDown("Action" + playerNumber))
            TryEat();

        inputHorizontal = Input.GetAxis(axisVertical);
        inputVertical = Input.GetAxis(axisHorizontal);

        if(inputHorizontal != 0 && playerAnim.GetBool("Run") != true) {
            playerAnim.SetBool("Run", true);
        }
        if(inputHorizontal == 0 && playerAnim.GetBool("Run") == true) {
            playerAnim.SetBool("Run", false);
        }

        if(starveBar.GetBar() <= 0 && !isDead) {
            isDead = true;
            playerAnim.enabled = false;
            transform.Rotate(Vector3.forward * -90f);
            Debug.Log("You are dead");
        }
    }

    void FixedUpdate()
    {
        Vector3 dir = Camera.main.transform.forward * inputHorizontal + Camera.main.transform.right * inputVertical;
        if(dir.magnitude != 0)
            dir /= dir.magnitude;
        rigidbody.velocity = dir;
    }
}
