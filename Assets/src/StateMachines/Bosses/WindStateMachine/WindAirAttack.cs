using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAirAttack : WindBaseState
{
    
    
    private bool alreadyAppliedForce;
    private AirAttackData attackAir;

  
    
    
    public WindAirAttack(WindStateMachine windstateMachine) : base(windstateMachine)
    {
        
        attackAir = windstateMachine.AirAttack;
    }

    public override void Enter()
    {
    
        windstateMachine.BossManager.Animator.CrossFadeInFixedTime(attackAir.AnimationName, attackAir.TransitionDuration);
        
    
    }

    public override void FixedTick(float fixeddeltaTime)
    {
    
         //We don't want to move when we are doing an attack on air
        windstateMachine.BossManager.RB.velocity = Vector2.zero;
            //apply force equial to the gravity to cancel the gravity when the player is on the air
        Vector2 v = Physics2D.gravity * windstateMachine.BossManager.RB.mass;
            //windstateMachine.BossManager.RB.velocity = -v;
        windstateMachine.BossManager.RB.AddForce(-v * 10f,ForceMode2D.Force);
    }

    public override void Tick(float deltaTime)
    {
    
        float normalizedTime = GetNormalizedTime(windstateMachine.BossManager.Animator);
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
            windstateMachine.SlashThrow(attackAir.Damage,attackAir.Knockback);
            
            //windstateMachine.SwitchState(new WindFallState(windstateMachine));
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
