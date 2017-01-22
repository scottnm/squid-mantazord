using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum AiAction
    {
        GoForward,
        TurnLeft,
        TurnRight,
        DoNothing
    }

    [System.Serializable]
    public struct AiAttitude
    {
        public float GoForwardRatio;
        public float TurnLeftRatio;
        public float TurnRightRatio;
        public float DoNothingRatio;
    }

    [System.Serializable]
    public struct AiMovement
    {
        public float moveSpeed;
        public float turnSpeed;
    }

    [System.Serializable]
    struct GameStateEvaluation
    {
        public Vector2 directionToPlayer;
        public bool canSeePlayer;
        public Vec2i tilePos;
        public bool canMoveForward;
        public bool canMoveBack;
        public bool canMoveRight;
        public bool canMoveLeft;
    }

    [SerializeField]
    AiMovement movement;
    [SerializeField]
    private AiAttitude attitude;
    [SerializeField]
    private AiAction currentAction = AiAction.DoNothing;
    [SerializeField]
    private float aiCooldownTime;
    private float aiCooldownTimer;
    [SerializeField]
    private Vector2 facingVector;
    [SerializeField]
    private Vector2 movementVector;
    [SerializeField]
    private GameStateEvaluation currentState;
    private GameObject player;

	private void Start ()
    {
        facingVector = Vector2.up;
        movementVector = Vector2.up;
        player = GameObject.FindGameObjectWithTag(Tags.Player);
        aiCooldownTimer = aiCooldownTime;
	}
	
	// Update is called once per frame
	private void Update ()
    {
        if (aiCooldownTimer <= 0)
        {
            // time to make another action
            EvaluteGameState();
            currentAction = Act(currentState);
            switch (currentAction)
            {
                case AiAction.GoForward:
                    movementVector = facingVector;
                    break;
                case AiAction.TurnLeft:
                    facingVector = Quaternion.Euler(0f, 0f, 90f) * facingVector;
                    movementVector = facingVector;
                    break;
                case AiAction.TurnRight:
                    facingVector = Quaternion.Euler(0f, 0f, -90f) * facingVector;
                    movementVector = facingVector;
                    break;
                case AiAction.DoNothing:
                    movementVector = Vector2.zero;
                    break;
            }
            // reset the timer
            aiCooldownTimer = aiCooldownTime;
        }
        else
        {
            aiCooldownTimer -= Time.deltaTime;
        }


        transform.Translate(movement.moveSpeed * movementVector * Time.deltaTime);
	}

    private void EvaluteGameState()
    {
        ArenaGrid grid = ArenaGenerator.GetGridInstance();
        currentState.directionToPlayer = player.transform.position - transform.position;
        currentState.canSeePlayer = Mathf.Abs(Vector2.Angle(facingVector, currentState.directionToPlayer)) < 90f;
        currentState.tilePos = Vec2i.toVec2i((Vector2)transform.position - grid.origin);

        var x = currentState.tilePos.x;
        var y = currentState.tilePos.y;


        float raycastDistance = 1.1f;

        currentState.canMoveForward = ! Physics2D.Raycast(transform.position, facingVector, raycastDistance, LayerMask.GetMask("Wall"));
        currentState.canMoveBack = ! Physics2D.Raycast(transform.position, -facingVector, raycastDistance, LayerMask.GetMask("Wall"));
        currentState.canMoveLeft = ! Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, 90) * facingVector, raycastDistance, LayerMask.GetMask("Wall"));
        currentState.canMoveRight =  ! Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -90) * facingVector, raycastDistance, LayerMask.GetMask("Wall"));

        Debug.DrawRay(transform.position, facingVector * raycastDistance, Color.red);
        Debug.DrawRay(transform.position, -facingVector * raycastDistance, Color.blue);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, 90) * facingVector * raycastDistance, Color.green);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, -90) * facingVector * 1.1f, Color.magenta);
    }

    private AiAction Act(GameStateEvaluation currentState)
    {
        float randomChoice = Random.Range(0f, 1f);
        if (randomChoice < attitude.GoForwardRatio)
        {
            if (currentState.canMoveForward)
            {
                return AiAction.GoForward;
            }
            else
            {
                if (currentState.canMoveLeft && currentState.canMoveRight)
                {
                    return Random.Range(0f, 1f) > 0.5f ? AiAction.TurnLeft : AiAction.TurnRight;
                }
                else if (currentState.canMoveRight)
                {
                    return AiAction.TurnRight;
                }
                return AiAction.TurnLeft;
            }
        }

        randomChoice -= attitude.GoForwardRatio;
        if (randomChoice < (attitude.TurnLeftRatio + attitude.TurnRightRatio))
        {
            return Random.Range(0f, 1f) > 0.5f ? AiAction.TurnLeft : AiAction.TurnRight;
        }

        return AiAction.DoNothing;
    }
}
