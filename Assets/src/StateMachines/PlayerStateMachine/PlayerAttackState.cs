using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
   

    private bool alreadyAppliedForce;
    private Attack attack;


    private Vector2 impact;

    private Vector2 dampingVelocity;

    private float attackDir;
    

    public PlayerAttackState(PlayerStateMachine stateMachine, int attackIdx) : base(stateMachine)
    {
        
        this.attackDir = stateMachine.isFacingRight? 1f:-1f;
        //we want to stop the current move input
        stateMachine.Horizontal = 0;
        attack = stateMachine.Attacks[attackIdx];
        stateMachine.CurrentAttackName = attack.AnimationName;
        SoundManager.PlaySound("swing");

    }

    public override void Enter()
    {
        stateMachine.IsAttacking = true;        
        setDamageWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        stateMachine.InputReader.RollEvent += OnRoll;
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        MoveAttack();  
    }

    public override void Tick(float deltaTime)
    {
        
        
        
        if(!stateMachine.IsGrounded()){
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
            return;
        }
         if(CanGuard()){
            OnGuard();
            return;
        } 
        
        //move the character taking into count the impulse force
        
        float normalizedTime = GetNormalizedTime(stateMachine.Animator);
        // this means that we are on an animation if is greater than 1 we are not doing nothing therefore we can change the state
        // we can change to the ground state
         // if the normal time is less than 1, it means that we are on an animation state
        if(normalizedTime < 1f){
         // if we are far enough the animation we can apply the forece
            if(normalizedTime >= attack.ForceTime){
                TryApplyForce();
            }
            
            // we double check if we are still attacking therefore we can change to the next state
            if(stateMachine.InputReader.isAttacking){
            
                TryNextCombo(normalizedTime);
            }
        
        }else{
            stateMachine.IsAttacking = false;   
            stateMachine.SwitchState(new PlayerRunState(stateMachine));
        }
        impact = Vector2.SmoothDamp(impact, Vector3.zero, ref dampingVelocity,stateMachine.Drag);
       
    }

     public override void Exit()
    {
        stateMachine.IsAttacking = false; 
        stateMachine.InputReader.RollEvent -= OnRoll;

    }



 

    private void TryNextCombo(float normalizedTime)
    {
         // check if we are on the last combo, lest remember that -1 means that is the last combo 
        if(attack.ComboStateIndex == -1){return;}
        //check if it has pass the time set for the combo
        // is the setted attack time is greater than the normalized time, therefore we are still performing an attack
        if(normalizedTime < attack.ComboAttackTime){return;}


        //  Debug.Log("This is the normalized time " + normalizedTime);
        // Debug.Log("This is the combo attack tiem " + attack.ComboAttackTime);
         stateMachine.SwitchState(
            new PlayerAttackState(
                stateMachine,
                attack.ComboStateIndex
            )
        );
        
    }

    private void TryApplyForce()
    {
        // we only want to add the foce once per animaiton
        // we this we ensure that is only added once
        if(alreadyAppliedForce){return;}
        // we want to add force to the player x direction times the force of the combo
        AddForce(attackDir * attack.Force);
        // if its added we can set it to true
        alreadyAppliedForce = true;
    }


    private void AddForce(float force){
        impact = new Vector2(force,stateMachine.RB.velocity.y);
    }

    private void MoveAttack(){
        //stateMachine.Horizontal = impact.x;
        stateMachine.RB.velocity = impact;
    }

    // this will call the set attack function of sword
    private void setDamageWeapon(){
        // this is for the sword therefore we can use it to handle the damage and the knockback
        if(stateMachine.Sword.TryGetComponent<SwordDamage>(out SwordDamage sword)){
            sword.SetAttack(attack.Damage,attack.Knockback);
        }

          //stateMachine.Weapon.SetAttack(attack.Damage, attack.Knockback);
    
    }

   

}
