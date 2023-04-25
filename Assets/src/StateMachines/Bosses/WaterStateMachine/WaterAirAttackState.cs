using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAirAttackState : WaterBaseState
{
    
    
    private bool alreadyAppliedForce;
    private AirAttackData attackAir;

    public WaterAirAttackState(WaterStateMachine waterstateMachine) : base(waterstateMachine)
    {
        attackAir = waterstateMachine.AirAttack;
    }

     public override void Enter()
    {
    
        waterstateMachine.BossManager.Animator.CrossFadeInFixedTime(attackAir.AnimationName, attackAir.TransitionDuration);
        
    
    }

    public override void FixedTick(float fixeddeltaTime)
    {
    
         //We don't want to move when we are doing an attack on air
        waterstateMachine.BossManager.RB.velocity = Vector2.zero;
            //apply force equial to the gravity to cancel the gravity when the player is on the air
        Vector2 v = Physics2D.gravity * waterstateMachine.BossManager.RB.mass;
            //waterstateMachine.BossManager.RB.velocity = -v;
        waterstateMachine.BossManager.RB.AddForce(-v * 10f,ForceMode2D.Force);
    }

    public override void Tick(float deltaTime)
    {
    
        float normalizedTime = GetNormalizedTime(waterstateMachine.BossManager.Animator);
        // this means that we are on an animation if is greater than 1 we are not doing nothing therefore we can change the state
        // we can change to the ground state
         // if the normal time is less than 1, it means that we are on an animation state
        //Debug.Log(normalizedTime);
        if(normalizedTime <= 1f){
         // if we are far enough the animation we can apply the forece
            if(normalizedTime >= attackAir.ForceTime){
                TryApplyForce();
            }
                    
        }else{
            //shoot
            waterstateMachine.SlashThrow(attackAir.Damage,attackAir.Knockback);
            
            //waterstateMachine.SwitchState(new WindFallState(waterstateMachine));
            OnRollAirAttack();
        }

    
    }
    public override void Exit()
    {
    }

    private void TryApplyForce()
    {
        // we only want to add the foce once per animaiton
        // we this we ensure that is only added once
        if(alreadyAppliedForce){return;}
        // we want to add force to the player x direction times the force of the combo
        // if its added we can set it to true
        alreadyAppliedForce = true;
    }


    private void AddForce(float force){
    }

    private void MoveAttack(){
        //stateMachine.Horizontal = impact.x;
    }

}
