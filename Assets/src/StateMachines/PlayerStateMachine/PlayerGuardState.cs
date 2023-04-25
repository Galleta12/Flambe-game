using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuardState : PlayerBaseState
{
    
    private readonly int DefendHash = Animator.StringToHash("Block");

    private const float CrossFadeDuration = 0.1f;

    private Vector2 impact;

    // damping velocity for the smoothdamp method
    private Vector2 dampingVelocity;
    // how long will take the impact stat
    
    
    public PlayerGuardState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.IsGuard = true;
        stateMachine.RB.velocity = Vector2.zero;
        stateMachine.Animator.CrossFadeInFixedTime(DefendHash, CrossFadeDuration);
        stateMachine.Health.setIsDefend(true);
        stateMachine.Health.OnGuardKnockback += AddForceImpact;
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        stateMachine.RB.velocity = new Vector2(impact.x, stateMachine.RB.velocity.y); 
    }

    public override void Tick(float deltaTime)
    {
        
        if(!CanGuard()){
            //stateMachine.RB.velocity = Vector2.zero; 
            stateMachine.SwitchState(new PlayerRunState(stateMachine));
        }
         impact = Vector2.SmoothDamp(impact, Vector2.zero, ref dampingVelocity,stateMachine.Drag);
    }
    public override void Exit()
    {
        stateMachine.IsGuard = false;
        stateMachine.Health.setIsDefend(false);
        stateMachine.Health.OnGuardKnockback -= AddForceImpact;



    }
     private void AddForceImpact(Vector2 force){
        impact += force;
        //Debug.Log("This is the impact" + impact);
    }

}
