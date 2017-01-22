using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartUIScript : MonoBehaviour
{

	public GameObject[] hearts = new GameObject[3];

	// Use this for initialization
	void Start ()
	{
		Events.OnPlayerHealthChange += HealthChange;
	}

	void HealthChange (int health)
	{
		for (int i = 0; i < 3; i++)
		{
			HeartAnimationScript has = hearts[i].GetComponent<HeartAnimationScript>();

			if (i < health)
			{
				has.ActivateHeart();
			}
			else
			{
				has.DeactivateHeart();
			}
		}
	}

	void OnDestroy ()
	{
		Events.OnPlayerHealthChange -= HealthChange;
	}
}
