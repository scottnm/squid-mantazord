using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour {

	public int health = 3;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionStay2D (Collision2D otherCollider)
	{
		GameObject otherGO = otherCollider.collider.gameObject;

		if (otherGO.tag == "Enemy")
		{
			health -= 1;
		}

		if (health <= 0)
		{
			gameObject.SetActive(false);
		}
	}
}
