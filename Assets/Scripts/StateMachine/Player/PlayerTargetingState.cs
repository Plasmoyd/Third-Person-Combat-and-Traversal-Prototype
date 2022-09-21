using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{

    private readonly int TARGETING_BLEND_TREE_HASH = Animator.StringToHash("TargetingBlendTree");
    private readonly int TARGETING_FORWARD_HASH = Animator.StringToHash("TargetingForward");
    private readonly int TARGETING_RIGHT_HASH = Animator.StringToHash("TargetingRight");

    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.CancelEvent += OnCancel;

        stateMachine.Animator.Play(TARGETING_BLEND_TREE_HASH);
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.CancelEvent -= OnCancel;
    }

    private void OnCancel()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private Vector3 CalculateMovement()
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;

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
