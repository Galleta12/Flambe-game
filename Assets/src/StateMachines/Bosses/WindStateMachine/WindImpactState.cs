using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindImpactState : WindBaseState
{
    
     //animator variables
    
    private readonly int ImpactHash = Animator.StringToHash("take_hit");
    private const float CrossFadeDuration = 0.1f;
    
    
    //--------------------------------------------
    private Vector2 currentKnockback;
    private Vector2 impact;

    // damping velocity for the smoothdamp method
    private Vector2 dampingVelocity;
    // how long will take the impact state
    private float duration;
    
    
    
    public WindImpactState(WindStateMachine windstateMachine, Vector2 diretionKnockBack) : base(windstateMachine)
    {
        this.currentKnockback = diretionKnockBack;
    }

    public override void Enter()
    {
        
        windstateMachine.BossManager.Animator.CrossFadeInFixedTime(ImpactHash,CrossFadeDuration);
        //we set the vector for the force impact
        AddForceImpact(currentKnockback);
        //Debug.Log("Impact State" );
        duration = windstateMachine.BossManager.HowLongImpact;
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        windstateMachine.BossManager.RB.velocity = impact;
    }

    public override void Tick(float deltaTime)
    {
        duration -= deltaTime;
       if(duration <= 0f){
            
            if(windstateMachine.BossManager.IsGrounded()){

                windstateMachine.SwitchState(new WindIdleState(windstateMachine));
                return;
            }else{
                windstateMachine.SwitchState(new WindFallState(windstateMachine));
                return;
            }
            
       }
       
        // need to know more about this, the dampingvelocity is only for this code, but we ensure to smootly go back to zero
        impact = Vector2.SmoothDamp(impact, Vector2.zero, ref dampingVelocity,windstateMachine.BossManager.Drag);
    }
    public override void Exit()
    {
        
    }

    private void AddForceImpact(Vector2 force){
        impact += force;
        //Debug.Log("This is the impact" + impact);
    }

}
