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

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
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

    protected void ReturnToLocomotion()
    {
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
    }
}
