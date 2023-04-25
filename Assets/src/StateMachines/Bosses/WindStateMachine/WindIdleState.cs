using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindIdleState : WindBaseState
{
    
    private readonly int IdleHash = Animator.StringToHash("idle");
    private const float CrossFadeDuration = 0.1f;

    private float remaingAttackForTime;
     private float remainingForRunning;

    
    public WindIdleState(WindStateMachine windstateMachine) : base(windstateMachine)
    {
    }

    public override void Enter()
    {
        remaingAttackForTime = windstateMachine.BossManager.HowLongBeforeAttack + 0.2f;
         remainingForRunning = windstateMachine.HowLongBeforeRunning;
        //windstateMachine.BossManager.Animator.CrossFadeInFixedTime(IdleHash,CrossFadeDuration);
         windstateMachine.BossManager.Animator.Play(IdleHash);
        windstateMachine.BossManager.RB.velocity = Vector2.zero;
        //Debug.Log("We are on the idle");

    }

    public override void FixedTick(float fixeddeltaTime)
    {
    }

    public override void Tick(float deltaTime)
    {
        
        
        
        
        windstateMachine.BossManager.FacePlayer();
        
        if(!windstateMachine.BossManager.IsGrounded()){
            windstateMachine.SwitchState(new WindFallState(windstateMachine));
            return;
        }
        
        if(windstateMachine.BossManager.PlayerMachine.IsJumping){
            windstateMachine.SwitchState(new WindRestingBeforeJump(windstateMachine));
            return;
        }

        //check if we can Guard
        CanDefend();
       
        
        //if(windstateMachine.CanAttack == true){return;}
        if(!windstateMachine.BossManager.IsPlayerInRange()){
             //if the player is nwot in range we want to change to the chase state
             
             remainingForRunning -=deltaTime;
            if(remainingForRunning <= 0f){

                windstateMachine.SwitchState(new WindChaseState(windstateMachine));
                return;
            }
             
             
        }else if(windstateMachine.BossManager.IsPlayerInRange()){
            
            remaingAttackForTime -= deltaTime;
            
            if(remaingAttackForTime<=0f){
                windstateMachine.CanAttack = true;
                windstateMachine.SwitchState(new WindRestingState(windstateMachine));
            }
            
           
        }
    }
    public override void Exit()
    {
        
    }

}
