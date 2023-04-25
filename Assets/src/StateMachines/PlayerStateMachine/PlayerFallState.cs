using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    
    private readonly int FallHash = Animator.StringToHash("Fall");

      private const float CrossFadeDuration = 0.1f;
    
    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
       // Debug.Log("Fall state");
        stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
        stateMachine.InputReader.RollEvent += OnRoll;
        stateMachine.IsFalling = true;
        
    }
    public override void FixedTick(float fixeddeltaTime)
    {
        
        stateMachine.RB.velocity = new Vector2(stateMachine.Horizontal * stateMachine.PlayerSpeed, stateMachine.RB.velocity.y);
        
    }

    public override void Tick(float deltaTime)
    {
        
        stateMachine.RB.velocity-= stateMachine.vecGravity * stateMachine.FallMultiplayer * deltaTime;
        
        stateMachine.Horizontal = stateMachine.InputReader.movementValueInputReader.x;
        
        
        if(stateMachine.IsGrounded()){
             
      
            stateMachine.SwitchState(new PlayerRunState(stateMachine));
            return;           
        }
        // if(stateMachine.InputReader.isAttacking && stateMachine.IsAirAttack){
        //     stateMachine.SwitchState(new PlayerAirAttackState(stateMachine));
        //     return;
        // }
    }

    public override void Exit()
    {
         stateMachine.InputReader.RollEvent -= OnRoll;
        stateMachine.IsFalling = false;

    }

}
