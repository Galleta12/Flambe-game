using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundIdleState : GroundBaseState
{
    
    
     private readonly int IdleHash = Animator.StringToHash("idle");

    private float remaingAttackForTime;

    private float remainingForRunning;
    
    public GroundIdleState(GroundStateMachine groundstateMachine) : base(groundstateMachine)
    {
    }

     public override void Enter()
    {
        remaingAttackForTime = groundstateMachine.BossManager.HowLongBeforeAttack + 0.2f;
        //windstateMachine.BossManager.Animator.CrossFadeInFixedTime(IdleHash,CrossFadeDuration);
        remainingForRunning = groundstateMachine.HowLongBeforeRunning;
        groundstateMachine.BossManager.Animator.Play(IdleHash);
        groundstateMachine.BossManager.RB.velocity = Vector2.zero;
        //Debug.Log("We are on the idle");

    }

    public override void FixedTick(float fixeddeltaTime)
    {
          groundstateMachine.BossManager.RB.velocity = Vector2.zero;
    }

    public override void Tick(float deltaTime)
    {
        
        
        
        //always look to the enemy
        groundstateMachine.BossManager.FacePlayer();
        
        if(!groundstateMachine.BossManager.IsGrounded()){
            groundstateMachine.SwitchState(new GroundFallState(groundstateMachine));
            return;
        }
        
      

        //check if we can Guard
        CanDefend();
       
        
   
        if(!groundstateMachine.BossManager.IsPlayerInRange()){
             //if the player is nwot in range we want to change to the chase state
             
             remainingForRunning -=deltaTime;
             if(remainingForRunning <= 0f){
                groundstateMachine.SwitchState(new GroundChaseState(groundstateMachine));
                return;
             }
            
             
        }
        else if(groundstateMachine.BossManager.IsPlayerInRange()){
            
            remaingAttackForTime -= deltaTime;
            
            if(remaingAttackForTime<=0f){
                groundstateMachine.CanAttack = true;
                groundstateMachine.SwitchState(new GroundRestState(groundstateMachine));
            }
            
        }
    }
     public override void Exit()
    {
        
    }
    
}
