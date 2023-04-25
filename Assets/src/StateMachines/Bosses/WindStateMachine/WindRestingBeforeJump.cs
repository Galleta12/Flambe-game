using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRestingBeforeJump : WindBaseState
{
    
    
    private readonly int IdleHash = Animator.StringToHash("Resting"); 
    private const float CrossFadeDuration = 0.1f;
    
    private float remainingTime;
    private bool shouldAirAttack;
    public WindRestingBeforeJump(WindStateMachine windstateMachine) : base(windstateMachine)
    {
    
    }

    public override void Enter()
    {
        
        this.remainingTime = windstateMachine.BossManager.HowLongBeforeJump;
        //windstateMachine.BossManager.Animator.CrossFadeInFixedTime(IdleHash,CrossFadeDuration);
        windstateMachine.BossManager.Animator.Play(IdleHash);
        windstateMachine.BossManager.RB.velocity = Vector2.zero;
        //get the air attack chance
        shouldAirAttack = AirAttackChance()? true:false;
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        //windstateMachine.BossManager.RB.velocity = Vector2.zero;
    }

    public override void Tick(float deltaTime)
    {
        remainingTime -= deltaTime;
        
        if(shouldAirAttack){
            //windstateMachine.UIAirAttack.SetActive(true);
        }

        if(remainingTime <=0){
            //windstateMachine.UIAirAttack.SetActive(false);
            windstateMachine.SwitchState(new WindJumpState(windstateMachine,shouldAirAttack));
        }
    }
    public override void Exit()
    {
    }

}
