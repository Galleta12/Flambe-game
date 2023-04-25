using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFallState : WindBaseState
{
    
    
    private readonly int FallHash = Animator.StringToHash("down");

    private const float CrossFadeDuration = 0.1f;
    
    public WindFallState(WindStateMachine windstateMachine) : base(windstateMachine)
    {
    }

    public override void Enter()
    {
         windstateMachine.BossManager.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }

    public override void FixedTick(float fixeddeltaTime)
    {
          windstateMachine.BossManager.RB.velocity-= windstateMachine.vecGravity * windstateMachine.BossManager.FallMultiplayer * fixeddeltaTime;
    }

    public override void Tick(float deltaTime)
    {
        windstateMachine.BossManager.FacePlayer();        
        if(windstateMachine.BossManager.IsGrounded()){
             
      
            windstateMachine.SwitchState(new WindIdleState(windstateMachine));
            return;           
        }
    }
    public override void Exit()
    {
        
    }

}
