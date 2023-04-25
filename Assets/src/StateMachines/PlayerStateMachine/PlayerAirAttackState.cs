using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttackState : PlayerBaseState
{
    
    
    
    
    private bool alreadyAppliedForce;
    private AirAttackData attackAir;

    private float attackDir;
    
    
    
    
    
    
    public PlayerAirAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        
        //for the the move we want to igoner the move when you are on the air attack
        stateMachine.IsAirAttackMove = true;
        //we want to stop the current move input
        stateMachine.Horizontal = 0;
        this.attackDir = stateMachine.isFacingRight? 1f:-1f;
        attackAir = stateMachine.AirAttack;
        //Debug.Log("This is the knockback" + attackAir.Knockback);
    }

    public override void Enter()
    {
        
//        setDamageWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(attackAir.AnimationName, attackAir.TransitionDuration);

    }

    public override void FixedTick(float fixeddeltaTime)
    {
            //We don't want to move when we are doing an attack on air
            stateMachine.RB.velocity = Vector2.zero;
            //apply force equial to the gravity to cancel the gravity when the player is on the air
            Vector2 v = Physics2D.gravity * stateMachine.RB.mass;
            //stateMachine.RB.velocity = -v;
            stateMachine.RB.AddForce(-v * 10f,ForceMode2D.Force);
    
    }

    public override void Tick(float deltaTime)
    {
        
        
        
        
        float normalizedTime = GetNormalizedTime(stateMachine.Animator);
        // this means that we are on an animation if is greater than 1 we are not doing nothing therefore we can change the state
        // we can change to the ground state
         // if the normal time is less than 1, it means that we are on an animation state
        if(normalizedTime <= 1f){
         // if we are far enough the animation we can apply the forece
            if(normalizedTime >= attackAir.ForceTime){
                TryApplyForce();
            }
                    
        }else{
            //shoot
            stateMachine.SlashThrow(attackAir.Damage,attackAir.Knockback);
            
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

       
    }

     public override void Exit()
    {
        stateMachine.IsAirAttackMove = false;
        stateMachine.IsAirAttack = false;        

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

    // this will call the set attack function of sword
    // private void setDamageWeapon(){
    //     // this is for the sword therefore we can use it to handle the damage and the knockback
    //     if(stateMachine.SlashObject.TryGetComponent<SlashAttack>(out SlashAttack slash)){
    //         slash.SetAttack(attackAir.Damage,attackAir.Knockback);
    //     }
    //      //stateMachine.Weapon.SetAttack(attack.Damage, attack.Knockback);
    
    // }

}
