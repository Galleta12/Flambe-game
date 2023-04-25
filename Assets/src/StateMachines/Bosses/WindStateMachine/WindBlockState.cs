using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlockState : WindBaseState
{
    
    private readonly int DefendHash = Animator.StringToHash("defend");

    private const float CrossFadeDuration = 0.1f;

    private float threashold=2f;

  

    private Vector2 impact;

    // damping velocity for the smoothdamp method
    private Vector2 dampingVelocity;
    // how long will take the impact state
    
    
    private float remaningTimeForAttack;
    
    public WindBlockState(WindStateMachine windstateMachine) : base(windstateMachine)
    {
        this.remaningTimeForAttack = windstateMachine.BossManager.HowLongBeforeAttack;
    }

    public override void Enter()
    {
        //Debug.Log("On block state");
        windstateMachine.BossManager.RB.velocity = Vector2.zero;
        windstateMachine.IsDefending = true;
        //windstateMachine.BossManager.Animator.CrossFadeInFixedTime(DefendHash, CrossFadeDuration);
        windstateMachine.BossManager.Animator.Play(DefendHash);

        windstateMachine.BossManager.Health.setIsDefend(true);
        //subscribe
        windstateMachine.BossManager.Health.OnGuardKnockback += AddForceImpact;
        
    }

    public override void FixedTick(float fixeddeltaTime)
    {
         windstateMachine.BossManager.RB.velocity = impact;
    }

    public override void Tick(float deltaTime)
    {
        
        windstateMachine.BossManager.FacePlayer();
        threashold -= deltaTime;
        remaningTimeForAttack -= deltaTime;
        if(remaningTimeForAttack <=0){
            
            //this is a transitation state
            
            windstateMachine.SwitchState(new WindRestingState(windstateMachine));
            windstateMachine.CanAttack = true;
            // windstateMachine.SwitchState(new WindAttackingState(windstateMachine,0));
        }
        
        if(threashold<=0){

            if(!windstateMachine.BossManager.PlayerMachine.IsAttacking){
                windstateMachine.SwitchState(new WindIdleState(windstateMachine));
            }
        }
        impact = Vector2.SmoothDamp(impact, Vector2.zero, ref dampingVelocity,windstateMachine.BossManager.Drag);
    }
    public override void Exit()
    {
        //Debug.Log("On exit block state");
        
        windstateMachine.BossManager.Health.setIsDefend(false);
        windstateMachine.IsDefending = false;
        windstateMachine.BossManager.Health.OnGuardKnockback -= AddForceImpact;

    }

    private void AddForceImpact(Vector2 force){
        impact += force;
        //Debug.Log("This is the impact" + impact);
    }

}
