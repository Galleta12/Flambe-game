using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBlockState : WaterBaseState
{
    
    
    
    private readonly int DefendHash = Animator.StringToHash("defend");

    private readonly int DefendOffHash = Animator.StringToHash("defendOff");



    private float threashold=2f;

    private Vector2 impact;

    // damping velocity for the smoothdamp method
    private Vector2 dampingVelocity;
    // how long will take the impact state
     private float remaningTimeForAttack;
    
    
    public WaterBlockState(WaterStateMachine waterstateMachine) : base(waterstateMachine)
    {
          this.remaningTimeForAttack = waterstateMachine.BossManager.HowLongBeforeAttack;
    }


    public override void Enter()
    {
        
        //Debug.Log("Is blocking");
        waterstateMachine.BossManager.RB.velocity = Vector2.zero;
        waterstateMachine.IsDefending = true;
        //waterstateMachine.BossManager.Animator.CrossFadeInFixedTime(DefendHash, CrossFadeDuration);
        waterstateMachine.BossManager.Animator.Play(DefendHash);

        waterstateMachine.BossManager.Health.setIsDefend(true);
        //subscribe
        waterstateMachine.BossManager.Health.OnGuardKnockback += AddForceImpact;
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        waterstateMachine.BossManager.RB.velocity = impact;
    }

    public override void Tick(float deltaTime)
    {
        
        //during alll the block we want to set the time, so we dont go to the healing
        if(!waterstateMachine.IsHealing){

            waterstateMachine.SetTimeForHeal = waterstateMachine.HowLongBeforeHeal;
        }
        
        waterstateMachine.BossManager.FacePlayer();
        threashold -= deltaTime;
        remaningTimeForAttack -= deltaTime;
        if(remaningTimeForAttack <=0){
            
            //this is a transitation state
            
            waterstateMachine.SwitchState(new WaterRestState(waterstateMachine));
            waterstateMachine.CanAttack = true;
            // windstateMachine.SwitchState(new WindAttackingState(windstateMachine,0));
        }
        
        if(threashold<=0){

            if(!IsPlayerAttacking()){
                waterstateMachine.BossManager.Animator.Play(DefendOffHash);
                waterstateMachine.SwitchState(new WaterIdleState(waterstateMachine));
                return;
            }
        }
        impact = Vector2.SmoothDamp(impact, Vector2.zero, ref dampingVelocity,waterstateMachine.BossManager.Drag); 
    }
    public override void Exit()
    {
        waterstateMachine.BossManager.Health.setIsDefend(false);
        waterstateMachine.IsDefending = false;
        waterstateMachine.BossManager.Health.OnGuardKnockback -= AddForceImpact;

    }

    private void AddForceImpact(Vector2 force){
        impact += force;
        //Debug.Log("This is the impact" + impact);
    }


}
