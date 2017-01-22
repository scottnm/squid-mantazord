using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
	public int health = 3;
	public string playerState = "healthy";
	public float hitFrameDuration = 2f;
	public float hitFrameValue;

	SpriteRenderer srHarpoonUL;
	SpriteRenderer srHarpoonUC;
	SpriteRenderer srHarpoonUR;
	SpriteRenderer srHarpoonML;
	SpriteRenderer srHarpoonMR;
	SpriteRenderer srHarpoonBL;
	SpriteRenderer srHarpoonBC;
	SpriteRenderer srHarpoonBR;

	// Use this for initialization
	void Start ()
	{
		hitFrameValue = hitFrameDuration;

		foreach (Transform i in transform)
		{
			if (i.name == "Anchor")
			{
				foreach (Transform j in i.transform)
				{
					if (j.name == "Harpoon_UL")
					{
						srHarpoonUL = j.gameObject.GetComponent<SpriteRenderer>();
					}
					else if (j.name == "Harpoon_UC")
					{
						srHarpoonUC = j.gameObject.GetComponent<SpriteRenderer>();
					}
					else if (j.name == "Harpoon_UR")
					{
						srHarpoonUR = j.gameObject.GetComponent<SpriteRenderer>();
					}
					else if (j.name == "Harpoon_ML")
					{
						srHarpoonML = j.gameObject.GetComponent<SpriteRenderer>();
					}
					else if (j.name == "Harpoon_MR")
					{
						srHarpoonMR = j.gameObject.GetComponent<SpriteRenderer>();
					}
					else if (j.name == "Harpoon_BL")
					{
						srHarpoonBL = j.gameObject.GetComponent<SpriteRenderer>();
					}
					else if (j.name == "Harpoon_BC")
					{
						srHarpoonBC = j.gameObject.GetComponent<SpriteRenderer>();
					}
					else if (j.name == "Harpoon_BR")
					{
						srHarpoonBR = j.gameObject.GetComponent<SpriteRenderer>();
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (playerState == "hurt")
		{
			hitFrameValue -= Time.deltaTime;
			float y = Mathf.Cos(8 * Mathf.PI * hitFrameValue);
			if (y >= 0)
			{
				gameObject.GetComponent<SpriteRenderer>().enabled = true;
				srHarpoonUL.enabled = true;
				srHarpoonUC.enabled = true;
				srHarpoonUR.enabled = true;
				srHarpoonML.enabled = true;
				srHarpoonMR.enabled = true;
				srHarpoonBL.enabled = true;
				srHarpoonBC.enabled = true;
				srHarpoonBR.enabled = true;
			}
			else
			{
				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				srHarpoonUL.enabled = false;
				srHarpoonUC.enabled = false;
				srHarpoonUR.enabled = false;
				srHarpoonML.enabled = false;
				srHarpoonMR.enabled = false;
				srHarpoonBL.enabled = false;
				srHarpoonBC.enabled = false;
				srHarpoonBR.enabled = false;
			}

			if (hitFrameValue <= 0)
			{
				hitFrameValue = hitFrameDuration;
				gameObject.GetComponent<SpriteRenderer>().enabled = true;
				srHarpoonUL.enabled = true;
				srHarpoonUC.enabled = true;
				srHarpoonUR.enabled = true;
				srHarpoonML.enabled = true;
				srHarpoonMR.enabled = true;
				srHarpoonBL.enabled = true;
				srHarpoonBC.enabled = true;
				srHarpoonBR.enabled = true;
				playerState = "healthy";
			}
		}
	}

	void OnCollisionStay2D (Collision2D otherCollider)
	{
		if (playerState == "healthy")
		{
			GameObject otherGO = otherCollider.collider.gameObject;

			if (otherGO.tag == "Enemy")
			{
				health -= 1;
				playerState = "hurt";
			}

			if (health <= 0)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
