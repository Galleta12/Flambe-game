using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterIdleState : WaterBaseState
{
    
    
    private readonly int IdleHash = Animator.StringToHash("idle");

    private float remaingAttackForTime;

    private float remainingForRunning;


    public WaterIdleState(WaterStateMachine waterstateMachine) : base(waterstateMachine)
    {
    }

    public override void Enter()
    {
        remaingAttackForTime = waterstateMachine.BossManager.HowLongBeforeAttack + 0.2f;
        //windstateMachine.BossManager.Animator.CrossFadeInFixedTime(IdleHash,CrossFadeDuration);
        remainingForRunning = waterstateMachine.HowLongBeforeRunning;
        waterstateMachine.BossManager.Animator.Play(IdleHash);
        waterstateMachine.BossManager.RB.velocity = Vector2.zero;
        //Debug.Log("We are on the idle");

    }

      public override void FixedTick(float fixeddeltaTime)
    {
    }

    public override void Tick(float deltaTime)
    {
        
        
        
        //always look to the enemy
        waterstateMachine.BossManager.FacePlayer();
        
        if(!waterstateMachine.BossManager.IsGrounded()){
            waterstateMachine.SwitchState(new WaterFallState(waterstateMachine));
            return;
        }
        
        if(IsPlayerJumping()){
            waterstateMachine.SwitchState(new WaterRestBeforeJumpState(waterstateMachine));
            return;
        }

        //check if we can Guard
        CanDefend();
       
        
        //if(windstateMachine.CanAttack == true){return;}
        if(!waterstateMachine.BossManager.IsPlayerInRange()){
             //if the player is nwot in range we want to change to the chase state
             
             remainingForRunning -=deltaTime;
             if(remainingForRunning <= 0f){
                waterstateMachine.SwitchState(new WaterSurfState(waterstateMachine));
                return;
             }
            
             
        }else if(waterstateMachine.BossManager.IsPlayerInRange()){
            
            remaingAttackForTime -= deltaTime;
            
            if(remaingAttackForTime<=0f){
                waterstateMachine.CanAttack = true;
                waterstateMachine.SwitchState(new WaterRestState(waterstateMachine));

            }
            
        }
    }
     public override void Exit()
    {
        
    }
    
}
