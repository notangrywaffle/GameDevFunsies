using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public GameObject Enemy;

    //Enemy stats
    private int attack = 1;
    private int health = 1;
    private int hitRange = 1;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void TakeDamage(int damageAmount)
    {
        health = health - damageAmount;

        if (health < 0)
        {
            // This enemy is supposed to be dead now.
            Enemy.SetActive(false);
        }
    }
}
