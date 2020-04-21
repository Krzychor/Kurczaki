using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameVictory : MonoBehaviour
{
    
    public Text text;
    public GameObject VictoryScreen;

    static GameVictory singleton;



    public static void SetWinner(string Winner)
    {
        singleton.gameObject.SetActive(true);
        singleton.text.text = "And the winner is " + Winner + "!";
    }

    private void Awake()
    {
        singleton = this;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.anyKeyDown)
            SceneManager.LoadScene("Menu");

    }
}
