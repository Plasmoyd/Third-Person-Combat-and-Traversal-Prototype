using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 movement, float deltaTime)
    {
        movement += stateMachine.ForceReceiver.Movement(); // this is calulating a movement vector by taking in consideration player input or motion vector, and adding gravity into equation

        stateMachine.CharacterController.Move(movement * deltaTime);
    }

    protected void FaceTarget()
    {
        if(stateMachine.Targeter.CurrentTarget == null) { return; }

        Vector3 lookDirection = (stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position);
        lookDirection.y = 0f;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookDirection);
    }
}
