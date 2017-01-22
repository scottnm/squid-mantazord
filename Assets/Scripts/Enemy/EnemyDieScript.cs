using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieScript : MonoBehaviour
{

	public void Die ()
	{
		gameObject.SetActive(false);
        Events.EnemyDies(gameObject);
	}

}
