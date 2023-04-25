using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRestBeforeJumpState : WaterBaseState
{
    
    
    //since walk doesn't seem like walk and I am using surf as walk
    //we are going to use this walk as idel
    private readonly int IdleHash = Animator.StringToHash("walk"); 
    private const float CrossFadeDuration = 0.1f;
    
    private float remainingTime;
    private bool shouldAirAttack;
    
    
    
    
    public WaterRestBeforeJumpState(WaterStateMachine waterstateMachine) : base(waterstateMachine)
    {
    }

    public override void Enter()
    {
        this.remainingTime = waterstateMachine.BossManager.HowLongBeforeJump;
        //windstateMachine.BossManager.Animator.CrossFadeInFixedTime(IdleHash,CrossFadeDuration);
        waterstateMachine.BossManager.Animator.Play(IdleHash);
        waterstateMachine.BossManager.RB.velocity = Vector2.zero;
        //get the air attack chance
        shouldAirAttack = AirAttackChance()? true:false;
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        
    }

    public override void Tick(float deltaTime)
    {
        remainingTime -= deltaTime;
        
        // if(shouldAirAttack){
        //     waterstateMachine.UIAirAttack.SetActive(true);
        // }

        if(remainingTime <=0){
           // waterstateMachine.UIAirAttack.SetActive(false);
            waterstateMachine.SwitchState(new WaterJumpState(waterstateMachine,shouldAirAttack));
        }
    }
    public override void Exit()
    {
        
    }

}
