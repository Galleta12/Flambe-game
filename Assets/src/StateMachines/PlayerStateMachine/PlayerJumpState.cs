using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    
    
    
     private readonly int JumpHash = Animator.StringToHash("Jump");

      private const float CrossFadeDuration = 0.1f;
    
    
    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
         //Debug.Log("Jump state");
        stateMachine.IsJumping = true;
        stateMachine.CreateDust();
        SoundManager.PlaySound("jump");
        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
        stateMachine.RB.velocity = Vector2.up * stateMachine.JumpSpeed;
        stateMachine.InputReader.RollEvent += OnRoll;
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        stateMachine.RB.velocity = new Vector2(stateMachine.Horizontal * stateMachine.PlayerSpeed, stateMachine.RB.velocity.y);
        
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.Horizontal = stateMachine.InputReader.movementValueInputReader.x;
        
        //No more Air Attack
        // if(stateMachine.InputReader.isAttacking && stateMachine.IsAirAttack){
        //     stateMachine.SwitchState(new PlayerAirAttackState(stateMachine));
        // }
        
        if(stateMachine.RB.velocity.y <=0){
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }   
    }
    public override void Exit()
    {
        stateMachine.IsJumping = false;
         stateMachine.InputReader.RollEvent -= OnRoll;
    }

}
