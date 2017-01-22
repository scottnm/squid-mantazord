using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonAttackScript : MonoBehaviour
{
	GameObject activeHarpoon = null;
	GameObject anchorObject = null;
	public Sprite retractedArm;
	public Sprite extendedArm;

	string harpoonState = "rest";
	float extendedArmDuration = .25f;
	float extendedArmValue;
	float cooldownDuration = .2f;
	float cooldownValue;

	[SerializeField]
	string harpoonName = "";

	// Use this for initialization
	void Start()
	{
		extendedArmValue = extendedArmDuration;
		cooldownValue = cooldownDuration;

		foreach (Transform ct in transform)
		{
			if (ct.gameObject.name == "Anchor")
			{
				anchorObject = ct.gameObject;
			}
		}

		foreach (Transform ct in anchorObject.transform)
		{
			if (ct.gameObject.name == "Harpoon_UC")
			{
				activeHarpoon = ct.gameObject;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (harpoonState == "cooldown")
		{
			cooldownValue -= Time.deltaTime;

			if (cooldownValue <= 0)
			{
				harpoonState = "rest";
				cooldownValue = cooldownDuration;
			}
		}

		else if (harpoonState == "rest")
		{
			var stickState = InputWrapper.GetRightStick ();

			if (stickState != InputWrapper.StickState.Center)
			{
				//select harpoon
				harpoonName = "";

				if (stickState == InputWrapper.StickState.Up)
				{
					harpoonName = "Harpoon_UC";
				}

				else if (stickState == InputWrapper.StickState.LeftUp)
				{
					harpoonName = "Harpoon_UL";
				}

				else if (stickState == InputWrapper.StickState.Left)
				{
					harpoonName = "Harpoon_ML";
				}

				else if (stickState == InputWrapper.StickState.LeftDown)
				{
					harpoonName = "Harpoon_BL";
				}

				else if (stickState == InputWrapper.StickState.Down)
				{
					harpoonName = "Harpoon_BC";
				}

				else if (stickState == InputWrapper.StickState.RightDown)
				{
					harpoonName = "Harpoon_BR";
				}

				else if (stickState == InputWrapper.StickState.Right)
				{
					harpoonName = "Harpoon_MR";
				}

				else if (stickState == InputWrapper.StickState.RightUp)
				{
					harpoonName = "Harpoon_UR";
				}

				foreach (Transform ct in anchorObject.transform)
				{
					if (ct.gameObject.name == harpoonName)
					{
						activeHarpoon = ct.gameObject;
					}
				}

				//activate specific harpoon
				var forwardVector = activeHarpoon.transform.TransformVector(Vector2.up);
				if (!Physics2D.Raycast(transform.position, forwardVector, 1.5f, LayerMask.GetMask("Wall")))
				{
					SpriteRenderer srA = activeHarpoon.GetComponent<SpriteRenderer>();
					srA.sprite = extendedArm;

					harpoonState = "attack";
				}
			}
		}

		else if (harpoonState == "attack")
		{
			var forwardVector = activeHarpoon.transform.TransformVector(Vector2.up);
			RaycastHit2D hit = Physics2D.Raycast(anchorObject.transform.position, forwardVector, 2f, LayerMask.GetMask("Enemy"));

			if (hit.collider != null)
			{
				GameObject hitGO = hit.collider.gameObject;
				EnemyDieScript eds = hitGO.GetComponent<EnemyDieScript>();
				eds.Die();
			}

			extendedArmValue -= Time.deltaTime;

			if (extendedArmValue <= 0)
			{
				SpriteRenderer srD = activeHarpoon.GetComponent<SpriteRenderer>();
				srD.sprite = retractedArm;

				harpoonState = "cooldown";
				extendedArmValue = extendedArmDuration;
			}
		}
	}
}
