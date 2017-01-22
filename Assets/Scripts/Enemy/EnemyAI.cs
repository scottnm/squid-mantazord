using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum AiAction
    {
        GoForward,
        TurnAndMove,
        Turn,
        DoNothing
    }

    [System.Serializable]
    public struct AiAttitude
    {
        public float GoForwardRatio;
        public float TurnAndMoveRatio;
        public float TurnRatio;
        public float DoNothingRatio;
    }

    struct GameStateEvaluation
    {
        public Vector2 directionToPlayer;
        public bool canSeePlayer;
        public Vec2i tilePos;
        public bool canMoveUp;
        public bool canMoveRight;
        public bool canMoveLeft;
        public bool canMoveDown;
    }

    [SerializeField]
    private AiAttitude attitude;
    [SerializeField]
    private AiAction currentAction = AiAction.DoNothing;
    [SerializeField]
    private float aiCooldownTime;
    [SerializeField]
    private float aiCooldownTimer;
    [SerializeField]
    private int actionCounts = 0;
    private Vector2 facingVector;
    private GameStateEvaluation currentState;
    private GameObject player;

	private void Start ()
    {
        facingVector = Vector2.up;
        player = GameObject.FindGameObjectWithTag(Tags.Player);
        aiCooldownTimer = aiCooldownTime;
	}
	
	// Update is called once per frame
	private void Update ()
    {
        if (aiCooldownTimer <= 0)
        {
            ++actionCounts;
            // time to make another action
            EvaluteGameState();
            currentAction = Act(currentState);
            switch (currentAction)
            {
                case AiAction.DoNothing:
                    break;
            }
            // reset the timer
            aiCooldownTimer = aiCooldownTime;
        }
        else
        {
            aiCooldownTimer -= Time.deltaTime;
        }
	}

    private void EvaluteGameState()
    {
        ArenaGrid grid = ArenaGenerator.GetGridInstance();
        currentState.directionToPlayer = player.transform.position - transform.position;
        currentState.canSeePlayer = Mathf.Abs(Vector2.Angle(facingVector, currentState.directionToPlayer)) < 90f;
        currentState.tilePos = Vec2i.toVec2i((Vector2)transform.position - grid.origin);

        var x = currentState.tilePos.x;
        var y = currentState.tilePos.y;
        currentState.canMoveDown = grid.IsWalkableTile(x, y - 1);
        currentState.canMoveUp = grid.IsWalkableTile(x, y + 1);
        currentState.canMoveLeft = grid.IsWalkableTile(x - 1, y);
        currentState.canMoveRight = grid.IsWalkableTile(x + 1, y);
    }

    private AiAction Act(GameStateEvaluation currentState)
    {
        float randomChoice = Random.Range(0f, 1f);
        if (randomChoice < attitude.GoForwardRatio)
        {
            return AiAction.GoForward;
        }

        randomChoice -= attitude.GoForwardRatio;
        if (randomChoice < attitude.TurnAndMoveRatio)
        {
            return AiAction.TurnAndMove;
        }

        randomChoice -= attitude.TurnAndMoveRatio;
        if (randomChoice < attitude.TurnRatio)
        {
            return AiAction.Turn;
        }

        return AiAction.DoNothing;
    }
}
