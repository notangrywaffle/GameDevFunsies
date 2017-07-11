using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesBehaviour : MonoBehaviour {

    public int Damage = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    void OnTriggerEnter2D(Collider2D other)
	{
        ApplyDamage(other.gameObject);
	}

	void OnTriggerStay2D(Collider2D other)
	{
        ApplyDamage(other.gameObject);
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        ApplyDamage(other.gameObject);
        Debug.Log(gameObject.name + " OnCollisionEnter with " + other.collider.name);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        ApplyDamage(other.gameObject);
        Debug.Log(gameObject.name + " OnCollisionStay with " + other.collider.name);
    }




    private void ApplyDamage(GameObject go)
	{
		try
		{
			PlayerController2D pc = go.GetComponent<PlayerController2D>();
            pc.RecieveDamage(Damage);
			
		}
		catch (System.Exception ex)
		{

		}
	}
}
