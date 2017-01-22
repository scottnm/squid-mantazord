using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffOnPursue : MonoBehaviour
{
    [SerializeField]
    private Sprite regular;
    [SerializeField]
    private Sprite puffed;
    private SpriteRenderer spriteRenderer;

    EnemyAI ai;
	private void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ai = GetComponent<EnemyAI>();
        ai.OnPursueStart += OnPursueStart;
        ai.OnPursueEnd += OnPursueEnd;
	}

    private void OnPursueStart()
    {
        spriteRenderer.sprite = puffed;
    }

    private void OnPursueEnd()
    {
        spriteRenderer.sprite = regular;
    }

}
