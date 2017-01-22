using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAnimationScript : MonoBehaviour
{

	public float frameDuration = .3f;
	float frameValue;
	public int frameIndex = 0;
	public int frameCount = 3;
	public Sprite[] heartFrames = new Sprite[3];
	public Sprite heartInactiveSprite;
	public string heartState = "active";

	// Use this for initialization
	void Start ()
	{
		frameValue = frameDuration;
	}

	// Update is called once per frame
	void Update ()
	{

		if (heartState == "active")
		{
			frameValue -= Time.deltaTime;

			if (frameValue <= 0)
			{
				frameIndex = (frameIndex+1) % frameCount;
				//gameObject.GetComponent<SpriteRenderer>().sprite = heartFrames[frameIndex];
				frameValue = frameDuration;
			}
		}
	}

	public void ActivateHeart () {
		if (heartState == "deactive")
		{
			heartState = "active";
			frameValue = frameDuration;
			gameObject.GetComponent<SpriteRenderer>().sprite = heartFrames[0];
		}
	}

	public void DeactivateHeart ()
	{
		heartState = "deactive";
		gameObject.GetComponent<SpriteRenderer>().sprite = heartInactiveSprite;
	}
}
