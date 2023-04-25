using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFallState : WaterBaseState
{
    
    private readonly int FallHash = Animator.StringToHash("down");

    private const float CrossFadeDuration = 0.1f;

    
    
    public WaterFallState(WaterStateMachine waterstateMachine) : base(waterstateMachine)
    {
      
    }

    public override void Enter()
    {
        waterstateMachine.BossManager.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }
    public override void FixedTick(float fixeddeltaTime)
    {
         waterstateMachine.BossManager.RB.velocity-= waterstateMachine.vecGravity * waterstateMachine.BossManager.FallMultiplayer * fixeddeltaTime;
    }

    public override void Tick(float deltaTime)
    {
        waterstateMachine.BossManager.FacePlayer();        
        if(waterstateMachine.BossManager.IsGrounded()){

          
             
      
            waterstateMachine.SwitchState(new WaterIdleState(waterstateMachine));
            return;           
        }
    }

    public override void Exit()
    {
    }

}
