using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{

    private readonly int FREE_LOOK_SPEED_HASH = Animator.StringToHash("FreeLookSpeed");
    private const float ANIMATOR_DAMP_TIME = .1f;

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Start()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        stateMachine.CharacterController.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero) 
        {
            stateMachine.Animator.SetFloat(FREE_LOOK_SPEED_HASH, 0, ANIMATOR_DAMP_TIME, deltaTime);
            return; 
        }

        stateMachine.Animator.SetFloat(FREE_LOOK_SPEED_HASH, 1, ANIMATOR_DAMP_TIME, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        
    }

    private Vector3 CalculateMovement()
    {
        Vector3 cameraForwardDirection = stateMachine.MainCameraTransform.forward;
        cameraForwardDirection.y = 0;
        cameraForwardDirection.Normalize();

        Vector3 cameraRightDirection = stateMachine.MainCameraTransform.right;
        cameraRightDirection.y = 0;
        cameraRightDirection.Normalize();

        return cameraForwardDirection * stateMachine.InputReader.MovementValue.y + cameraRightDirection * stateMachine.InputReader.MovementValue.x;

    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), stateMachine.RotationSmoothValue * deltaTime);
    }

}
