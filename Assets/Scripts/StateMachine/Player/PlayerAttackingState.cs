using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private const float CROSS_FADE_DURATION = .1f;

    private Attack attack;
    private float previousFrameTime;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
    {
        attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Weapon.SetAttack(attack.Damage);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, CROSS_FADE_DURATION);
    }

    public override void Tick(float deltaTime)
    {

        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedAnimationTime();

        if( normalizedTime < 1f)
        {
            if(stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if(stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
    }

    private void TryComboAttack(float normalizedTime)
    {
        if(attack.ComboStateIndex == -1) { return; }

        if(normalizedTime < attack.ComboAttackTime) { return; }

        stateMachine.SwitchState(new PlayerAttackingState(

            stateMachine,
            attack.ComboStateIndex
            ));
    }

    private float GetNormalizedAnimationTime()
    {

        AnimatorStateInfo currentStateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextStateInfo = stateMachine.Animator.GetNextAnimatorStateInfo(0);

        if(stateMachine.Animator.IsInTransition(0) && nextStateInfo.IsTag("Attack"))
        {
            return nextStateInfo.normalizedTime;
        }
        else if(!stateMachine.Animator.IsInTransition(0) && currentStateInfo.IsTag("Attack"))
        {
            return currentStateInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
