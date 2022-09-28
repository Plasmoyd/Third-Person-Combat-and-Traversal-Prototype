using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected bool IsPlayerInChaseRange()
    {
        float distance = CalculateDistanceToPlayer();

        return distance <= stateMachine.PlayerChasingRange;
    }

    protected bool IsPlayerInAttackRange()
    {
        float distance = CalculateDistanceToPlayer();

        return distance <= stateMachine.AttackRange;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 movement, float deltaTime)
    {

        movement += stateMachine.ForceReceiver.Movement();
        stateMachine.Controller.Move(movement * deltaTime);
    }

    protected void RotateToPlayer()
    {
        if(stateMachine.Player == null) { return; }

        Vector3 directionVector = (stateMachine.Player.transform.position - stateMachine.transform.position).normalized;
        directionVector.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(directionVector);
    }

    private float CalculateDistanceToPlayer()
    {
        GameObject player = stateMachine.Player;

        return Vector3.Distance(player.transform.position, stateMachine.transform.position);
    }
}
