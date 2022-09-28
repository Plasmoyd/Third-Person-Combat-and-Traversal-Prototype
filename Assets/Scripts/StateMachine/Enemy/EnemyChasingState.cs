using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{

    private readonly int LOCOMOTION_BLEND_TREE_HASH = Animator.StringToHash("LocomotionBlendTree");
    private readonly int LOCOMOTION_SPEED_HASH = Animator.StringToHash("Speed");

    private const float CROSS_FADE_DURATION = .1f;
    private const float ANIMATOR_DAMPING_VALUE = .1f;

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LOCOMOTION_BLEND_TREE_HASH, CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime)
    {

        if(!IsPlayerInChaseRange())
        {
            //switch to idle state
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        else if(IsPlayerInAttackRange())
        {
            //switch to attacking state.
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);
        RotateToPlayer();

        stateMachine.Animator.SetFloat(LOCOMOTION_SPEED_HASH, 1, ANIMATOR_DAMPING_VALUE, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.NavAgent.ResetPath();
        stateMachine.NavAgent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltraTime)
    {
        if (stateMachine.NavAgent.isOnNavMesh)
        {
            stateMachine.NavAgent.destination = stateMachine.Player.transform.position;

            Move(stateMachine.NavAgent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltraTime);
        }
        
        stateMachine.NavAgent.velocity = stateMachine.Controller.velocity;
    }

}
