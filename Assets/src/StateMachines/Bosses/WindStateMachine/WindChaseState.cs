using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindChaseState : WindBaseState
{
    
    private readonly int RunHash = Animator.StringToHash("run"); 
    

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;
    
    
    public WindChaseState(WindStateMachine windstateMachine) : base(windstateMachine)
    {
    }

    public override void Enter()
    {
        //Debug.Log("Is chasing the boss");
        //windstateMachine.BossManager.Animator.CrossFadeInFixedTime(RunHash,CrossFadeDuration); 
        windstateMachine.BossManager.Animator.Play(RunHash);
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
    
        
        if(windstateMachine.BossManager.PlayerMachine.IsJumping){
            windstateMachine.SwitchState(new WindRestingBeforeJump(windstateMachine));
            return;
        }
        
        
        CanDefend();
        //check if we can Guard
        windstateMachine.BossManager.FacePlayer();

        //Ignore the y since we dont want the enemy to move up
        Vector2 moveTo = new Vector2(windstateMachine.BossManager.PlayerGameObjct.transform.position.x, windstateMachine.transform.position.y);
        
        windstateMachine.transform.position =Vector2.MoveTowards(windstateMachine.transform.position,moveTo,windstateMachine.BossManager.Speed * deltaTime);
        
        if(windstateMachine.BossManager.PlayerMachine.IsJumping){
            windstateMachine.SwitchState(new WindIdleState(windstateMachine));
        }
        
        if(windstateMachine.BossManager.IsPlayerInRange()){
           
            
            windstateMachine.SwitchState(new WindIdleState(windstateMachine));
            //windstateMachine.CanAttack = true;
            //windstateMachine.SwitchState(new WindAttackingState(windstateMachine,0));
        }
     
       

    }
    public override void Exit()
    {
        
    }

}
