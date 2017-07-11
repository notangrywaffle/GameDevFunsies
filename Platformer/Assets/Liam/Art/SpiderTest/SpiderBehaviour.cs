using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBehaviour : MonoBehaviour {

	public enum ActorState { Alive, Dead }

	public int Damage = 1;
	public int Health = 1;

	private ActorState spiderState = ActorState.Alive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (spiderState == ActorState.Dead)
		{
			this.gameObject.SetActive(false);
		}
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		PlayerController2D player = other.GetComponent<PlayerController2D>();
		if ((player != null) && (player.Motor.motorState == PlatformerMotor2D.MotorState.AttackSwipe))
		{
            Debug.LogWarning("deal damage to spider");
			this.Health -= player.PlayerDamage;
			if (this.Health < 1)
			{
				spiderState = ActorState.Dead;
			}
		}
		else if (spiderState != ActorState.Dead)
		{
            Debug.LogWarning("deal damage to player");
			ApplyDamage(other.gameObject);
		}
		
		
	}

	void OnTriggerStay2D(Collider2D other)
	{
		//ApplyDamage(other.gameObject);
	}

	//void OnCollisionEnter2D(Collision2D other)
	//{
	//    ApplyDamage(other.gameObject);
	//    Debug.Log(gameObject.name + " OnCollisionEnter with " + other.collider.name);
	//}

	//void OnCollisionStay2D(Collision2D other)
	//{
	//    ApplyDamage(other.gameObject);
	//    Debug.Log(gameObject.name + " OnCollisionStay with " + other.collider.name);
	//}




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
