using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{

    private readonly int TARGETING_BLEND_TREE_HASH = Animator.StringToHash("TargetingBlendTree");
    private readonly int TARGETING_FORWARD_HASH = Animator.StringToHash("TargetingForward");
    private readonly int TARGETING_RIGHT_HASH = Animator.StringToHash("TargetingRight");

    private const float CROSS_FADE_DURATION = .1f;

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnCancel;
        stateMachine.InputReader.DodgeEvent += OnDodge;

        stateMachine.Animator.CrossFadeInFixedTime(TARGETING_BLEND_TREE_HASH, CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if(stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnCancel;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
    }

    private void OnCancel()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private void OnDodge()
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero) { return; }

        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.forward.normalized * stateMachine.InputReader.MovementValue.y;
        movement += stateMachine.transform.right.normalized * stateMachine.InputReader.MovementValue.x;

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        Vector2 movementValue = stateMachine.InputReader.MovementValue;
        float forwardValue = movementValue.y;
        float rightValue = movementValue.x;

        switch(forwardValue)
        {
            case < 0:
                stateMachine.Animator.SetFloat(TARGETING_FORWARD_HASH, -1f , .1f, deltaTime);
                break;
            case 0:
                stateMachine.Animator.SetFloat(TARGETING_FORWARD_HASH, 0f, .1f, deltaTime);
                break;
            case > 0:
                stateMachine.Animator.SetFloat(TARGETING_FORWARD_HASH, 1f, .1f, deltaTime);
                break;
        }

        switch(rightValue)
        {
            case < 0:
                stateMachine.Animator.SetFloat(TARGETING_RIGHT_HASH, -1f, .1f, deltaTime);
                break;
            case 0:
                stateMachine.Animator.SetFloat(TARGETING_RIGHT_HASH, 0f, .1f, deltaTime);
                break;
            case > 0:
                stateMachine.Animator.SetFloat(TARGETING_RIGHT_HASH, 1f, .1f, deltaTime);
                break;
        }
    }
}
