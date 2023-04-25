using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFallState : GroundBaseState
{
    
    private readonly int FallHash = Animator.StringToHash("down");

    private const float CrossFadeDuration = 0.1f;
    
    public GroundFallState(GroundStateMachine groundstateMachine) : base(groundstateMachine)
    {
    }

    public override void Enter()
    {
        groundstateMachine.BossManager.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }
    public override void FixedTick(float fixeddeltaTime)
    {
         groundstateMachine.BossManager.RB.velocity-= groundstateMachine.vecGravity * groundstateMachine.BossManager.FallMultiplayer * fixeddeltaTime;
    }

    public override void Tick(float deltaTime)
    {
        groundstateMachine.BossManager.FacePlayer();        
        if(groundstateMachine.BossManager.IsGrounded()){

          
             
      
            groundstateMachine.SwitchState(new GroundIdleState(groundstateMachine));
            return;           
        }
    }

    public override void Exit()
    {
    }
}
