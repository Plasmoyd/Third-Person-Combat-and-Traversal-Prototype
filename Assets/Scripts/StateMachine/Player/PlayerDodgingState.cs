using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{

    private readonly int DODGING_BLEND_TREE_HASH = Animator.StringToHash("DodgingBlendTree");
    private readonly int DODGE_FORWARD_HASH = Animator.StringToHash("DodgeForward");
    private readonly int DODGE_RIGHT_HASH = Animator.StringToHash("DodgeRight");

    private const float CROSS_FADE_DURATION = .1f;

    private Vector3 dodgingDirectionInput;
    private float remainingDodgeTime;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
    {
        this.dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;

        stateMachine.Animator.SetFloat(DODGE_FORWARD_HASH, dodgingDirectionInput.y);
        stateMachine.Animator.SetFloat(DODGE_RIGHT_HASH, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DODGING_BLEND_TREE_HASH, CROSS_FADE_DURATION);

        stateMachine.Health.SetIsInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.forward.normalized * dodgingDirectionInput.y * stateMachine.DodgeLength / stateMachine.DodgeDuration;
        movement += stateMachine.transform.right.normalized * dodgingDirectionInput.x * stateMachine.DodgeLength / stateMachine.DodgeDuration;

        Move(movement, deltaTime);
        FaceTarget();

        remainingDodgeTime -= deltaTime;

        if(remainingDodgeTime <= 0f)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetIsInvulnerable(false);
    }

}
