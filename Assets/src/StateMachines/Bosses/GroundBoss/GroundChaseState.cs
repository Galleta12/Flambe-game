using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChaseState : GroundBaseState
{
    
     private readonly int RunHash = Animator.StringToHash("run"); 
    public GroundChaseState(GroundStateMachine groundstateMachine) : base(groundstateMachine)
    {
    }
    public override void Enter()
    {
        groundstateMachine.BossManager.Animator.Play(RunHash);
    }
    public override void FixedTick(float fixeddeltaTime)
    {
        
    }

    public override void Tick(float deltaTime)
    {
        
        groundstateMachine.BossManager.FacePlayer();
          
        
        CanDefend();

        //Ignore the y since we dont want the enemy to move up
        Vector2 moveTo = new Vector2(groundstateMachine.BossManager.PlayerGameObjct.transform.position.x, groundstateMachine.transform.position.y);
        
        groundstateMachine.transform.position =Vector2.MoveTowards(groundstateMachine.transform.position,moveTo,groundstateMachine.BossManager.Speed * deltaTime);
        
        if(groundstateMachine.BossManager.IsPlayerInRange()){
           
            
            groundstateMachine.SwitchState(new GroundIdleState(groundstateMachine));
            
        }
      
    }
    public override void Exit()
    {
        
    }

}
