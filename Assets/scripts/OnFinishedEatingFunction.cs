using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnFinishedEatingFunction : MonoBehaviour
{
    public void OnFinishedEating()
    {
        ChickenPlayer CP = transform.parent.GetComponent<ChickenPlayer>();
        CP.Eat();
    }



}
