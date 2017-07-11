using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public int Value = 1;

    void Start()
    {
        //if (IsFirstInLevel)
        //{
        //    PlayerController2D pc = GameObject.Find("Basic Player Controller").GetComponent<PlayerController2D>();
        //    if (pc != null)
        //    {
        //        SetCheckPoint(pc);
        //    }
        //    else { Debug.LogWarning("Didnt' find the player..."); }
        //}
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController2D pc = other.gameObject.GetComponent<PlayerController2D>();
        if (pc != null)
        {
            pc.PickedUpCoin(Value);
            this.gameObject.SetActive(false);
        }
        
    }


  
}
