using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{

    private readonly int LOCOMOTION_BLEND_TREE_HASH = Animator.StringToHash("LocomotionBlendTree");
    private readonly int LOCOMOTION_SPEED_HASH = Animator.StringToHash("Speed");

    private const float CROSS_FADE_DURATION = .1f;
    private const float ANIMATOR_DAMPING_VALUE = .1f;

    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LOCOMOTION_BLEND_TREE_HASH, CROSS_FADE_DURATION);
        
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Animator.SetFloat(LOCOMOTION_SPEED_HASH, 0 , ANIMATOR_DAMPING_VALUE, deltaTime);
    }

    public override void Exit()
    {
        
    }

}
