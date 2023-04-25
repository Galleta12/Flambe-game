using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    
    
         //animator variables
    
    private readonly int ImpactHash = Animator.StringToHash("Impact");
    private const float CrossFadeDuration = 0.1f;
    
    
    //--------------------------------------------
    private Vector2 currentKnockback;
    private Vector2 impact;

    // damping velocity for the smoothdamp method
    private Vector2 dampingVelocity;
    // how long will take the impact state
    private float duration;
    
    
    public PlayerImpactState(PlayerStateMachine stateMachine, Vector2 diretionKnockBack) : base(stateMachine)
    {
         this.currentKnockback = diretionKnockBack;
    }

    public override void Enter()
    {
        stateMachine.IsImpact = true;
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash,CrossFadeDuration);
        //we set the vector for the force impact
        AddForceImpact(new Vector2(currentKnockback.x, stateMachine.RB.velocity.y));
        //Debug.Log("Impact State" );
        duration = stateMachine.HowLongImpact;
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        stateMachine.RB.velocity = impact;
    }

    public override void Tick(float deltaTime)
    {
       duration -= deltaTime;
       if(duration <= 0f){
            
            if(stateMachine.IsGrounded()){

                stateMachine.SwitchState(new PlayerRunState(stateMachine));
                return;
            }else{
                stateMachine.SwitchState(new PlayerFallState(stateMachine));
                return;
            }
       }
        // need to know more about this, the dampingvelocity is only for this code, but we ensure to smootly go back to zero
      impact = Vector2.SmoothDamp(impact, Vector2.zero, ref dampingVelocity,stateMachine.Drag); 
    }
    public override void Exit()
    {
        stateMachine.IsImpact = false;
    }
    private void AddForceImpact(Vector2 force){
        impact += force;
        //Debug.Log("This is the impact" + impact);
    }

}
