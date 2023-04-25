using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBlockState : GroundBaseState
{
    
    private readonly int DefendHash = Animator.StringToHash("defend");
    private float threashold=2f;

    private Vector2 impact;

    // damping velocity for the smoothdamp method
    private Vector2 dampingVelocity;
    // how long will take the impact state
    private float remaningTimeForAttack;
    
    public GroundBlockState(GroundStateMachine groundstateMachine) : base(groundstateMachine)
    {
        this.remaningTimeForAttack = groundstateMachine.BossManager.HowLongBeforeAttack;
    }

    public override void Enter()
    {
        
        //Debug.Log("Is blocking");
        groundstateMachine.BossManager.RB.velocity = Vector2.zero;
        groundstateMachine.IsDefending = true;
        //groundstateMachine.BossManager.Animator.CrossFadeInFixedTime(DefendHash, CrossFadeDuration);
        groundstateMachine.BossManager.Animator.Play(DefendHash);

        groundstateMachine.BossManager.Health.setIsDefend(true);
        //subscribe
        groundstateMachine.BossManager.Health.OnGuardKnockback += AddForceImpact;
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        groundstateMachine.BossManager.RB.velocity = impact;
    }

    public override void Tick(float deltaTime)
    {
        
        //during alll the block we want to set the time, so we dont go to the healing
        if(!groundstateMachine.IsHealing){

            groundstateMachine.SetTimeForHeal = groundstateMachine.HowLongBeforeHeal;
        }
        
        groundstateMachine.BossManager.FacePlayer();
        threashold -= deltaTime;
        remaningTimeForAttack -= deltaTime;
        if(remaningTimeForAttack <=0){
            
            //this is a transitation state
            
            groundstateMachine.SwitchState(new GroundRestState(groundstateMachine));
            groundstateMachine.CanAttack = true;
           
        }
        
        if(threashold<=0){

            if(!IsPlayerAttacking()){
                groundstateMachine.SwitchState(new GroundIdleState(groundstateMachine));
                return;
            }
        }
        impact = Vector2.SmoothDamp(impact, Vector2.zero, ref dampingVelocity,groundstateMachine.BossManager.Drag); 
    }
    public override void Exit()
    {
        groundstateMachine.BossManager.Health.setIsDefend(false);
        groundstateMachine.IsDefending = false;
        groundstateMachine.BossManager.Health.OnGuardKnockback -= AddForceImpact;

    }

    private void AddForceImpact(Vector2 force){
        impact += force;
        //Debug.Log("This is the impact" + impact);
    }

}
