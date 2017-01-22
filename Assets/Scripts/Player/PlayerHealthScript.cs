using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    enum PlayerState
    {
       Healthy,
       Hurt,
       Dead
    }

    [SerializeField]
    private int maxHealth;
	private int health;
	private float hitFrameDuration = 2f;
	private PlayerState state;
	private float hitFrameValue;
    private SpriteRenderer[] renderers;

	void Start ()
	{
        Reset();
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
	}

    private void Reset()
    {
        state = PlayerState.Healthy;
        health = maxHealth;
        hitFrameValue = hitFrameDuration;
    }

	void Update ()
	{
		if (state == PlayerState.Hurt)
		{
			hitFrameValue -= Time.deltaTime;
			float y = Mathf.Cos(8 * Mathf.PI * hitFrameValue);
            bool hurtTimerUp = hitFrameValue <= 0;
            SetRenderers(y >= 0 || hurtTimerUp);
			if (hurtTimerUp)
			{
				hitFrameValue = hitFrameDuration;
                state = PlayerState.Healthy;
			}
		}
	}

    private void SetRenderers(bool enabled)
    {
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.enabled = enabled;
        }
    }

	void OnCollisionStay2D (Collision2D otherCollider)
	{
		if (state == PlayerState.Healthy)
		{
			GameObject otherGO = otherCollider.collider.gameObject;

			if (otherGO.tag == Tags.Enemy)
			{
				health -= 1;
				state = PlayerState.Hurt;
                Events.PlayerDamaged();
                Events.PlayerHealthChanges(health);
			}

			if (health <= 0)
			{
                gameObject.SetActive(false);
                Events.PlayerDies();
			}
		}
	}
}
