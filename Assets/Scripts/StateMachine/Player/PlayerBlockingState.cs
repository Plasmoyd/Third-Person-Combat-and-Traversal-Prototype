using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BLOCKING_IDLE_HASH = Animator.StringToHash("BlockingIdle");

    private const float CROSS_FADE_DURATION = .1f;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(BLOCKING_IDLE_HASH, CROSS_FADE_DURATION);
        stateMachine.Health.SetIsInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if(!stateMachine.InputReader.IsBlocking)
        {
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetIsInvulnerable(false);
    }

}
