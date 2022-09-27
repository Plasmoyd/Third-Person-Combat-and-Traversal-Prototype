using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }
    public bool IsAttacking { get; private set; }

    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action CancelEvent;

    private Controls controls;

    void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }

        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }

        TargetEvent?.Invoke();
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }

        CancelEvent?.Invoke();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {

        /* 
         CONSIDER REFACTORING THIS METHOD. I WANT PLAYER TO CLICK EVERY SINGLE TIME THEY WANT TO ATTACK, WHILE IN COURSE THEY WANT TO
         ENABLE PLAYER TO JUST HOLD DOWN THE BUTTON, AND DO THE COMBO
         */

        if(context.performed)
        {
            IsAttacking = true;
        }
        else if(context.canceled)
        {
            IsAttacking = false;
        }
    }
}
