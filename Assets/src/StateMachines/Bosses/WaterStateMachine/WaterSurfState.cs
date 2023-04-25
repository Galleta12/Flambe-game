using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurfState : WaterBaseState
{
    
    
    
    private readonly int RunHash = Animator.StringToHash("surf"); 
    
      
    public WaterSurfState(WaterStateMachine waterstateMachine) : base(waterstateMachine)
    {
    }

    public override void Enter()
    {
         waterstateMachine.BossManager.Animator.Play(RunHash);
    }
    public override void FixedTick(float fixeddeltaTime)
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
        waterstateMachine.BossManager.FacePlayer();
        
        
        
      
        
        if(IsPlayerJumping()){
            waterstateMachine.SwitchState(new WaterRestBeforeJumpState(waterstateMachine));
            return;
        }
        
        CanDefend();

        //Ignore the y since we dont want the enemy to move up
        Vector2 moveTo = new Vector2(waterstateMachine.BossManager.PlayerGameObjct.transform.position.x, waterstateMachine.transform.position.y);
        
        waterstateMachine.transform.position =Vector2.MoveTowards(waterstateMachine.transform.position,moveTo,waterstateMachine.BossManager.Speed * deltaTime);
        
        if(waterstateMachine.BossManager.IsPlayerInRange()){
           
            
            waterstateMachine.SwitchState(new WaterIdleState(waterstateMachine));
            
        }
      
    }
    public override void Exit()
    {
        
    }


}
