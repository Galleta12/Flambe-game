using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttackingState : GroundBaseState
{
    private bool alreadyAppliedForce;
    private Attack attack;


    private Vector2 impact;

    private Vector2 dampingVelocity;

    private float attackDir;

    private int _attackIdx;

   
    
    
    
    public GroundAttackingState(GroundStateMachine groundstateMachine, int attackIdx) : base(groundstateMachine)
    {
         this.attackDir = groundstateMachine.BossManager.FacingLeft? -1f:1f;
        this._attackIdx = attackIdx;
        //we want to stop the current move input
        //groundstateMachine.BossManager.RB.velocity = Vector2.zero;
       
        attack = groundstateMachine.BossManager.Attacks[attackIdx];
        //Debug.Log("This is the next attack "+ attack.AnimationName);
          //if it was set up as true we can exit it and set it up as false
    }

    public override void Enter()
    {
        //we want to face the player
        
        
        groundstateMachine.BossManager.FacePlayer();
        setDamageWeapon();
        //Debug.Log("Attack ind " + _attackIdx );
        groundstateMachine.BossManager.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        groundstateMachine.BossManager.RB.velocity = Vector2.zero;
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        MoveAttack();  
    }

    public override void Tick(float deltaTime)
    {
        
        
        float normalizedTime = GetNormalizedTime(groundstateMachine.BossManager.Animator);
        if(normalizedTime < 1f){
            //Debug.Log("less that normalized");
         // if we are far enough the animation we can apply the forece
            if(normalizedTime >= attack.ForceTime){
                TryApplyForce();
              //  Debug.Log("apply force");
            }

             TryNextCombo(normalizedTime); 
        }else{
           
            groundstateMachine.CanAttack = false;
            //Debug.Log("NOthing");
            groundstateMachine.SwitchState(new GroundIdleState(groundstateMachine));
            
        }
        impact = Vector2.SmoothDamp(impact, Vector3.zero, ref dampingVelocity,groundstateMachine.BossManager.Drag);
    }
    public override void Exit()
    {
      
    }

    private void TryNextCombo(float normalizedTime)
    {
         // check if we are on the last combo, lest remember that -1 means that is the last combo 
        if(attack.ComboStateIndex == -1){  
            //Debug.Log("We are on the last combo");
            
            if(normalizedTime > attack.ComboAttackTime){
                
               //Debug.Log("We are on the last combo and we can change");
                if(AttackContinue() &&  !IsPlayerJumping()){
                    
                    groundstateMachine.CanAttack = true; 
                    groundstateMachine.SwitchState(new GroundRestState(groundstateMachine));
                }
                else{
                    
                    groundstateMachine.CanAttack = false;
                    groundstateMachine.SwitchState(new GroundIdleState(groundstateMachine));
                    return;
                }
            }
            return;
        }
        //check if it has pass the time set for the combo
        // is the setted attack time is greater than the normalized time, therefore we are still performing an attack
        //We are using 0.98f here as the combo attack time
        if(normalizedTime < attack.ComboAttackTime){return;}
        //Debug.Log("Co to next attack");
        // Debug.Log("This is the normalized time " + normalizedTime);
        // Debug.Log("This is the combo attack tiem " + attack.ComboAttackTime);

        groundstateMachine.SwitchState(
            new GroundAttackingState(
                groundstateMachine,
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
        impact = new Vector2(force,groundstateMachine.BossManager.RB.velocity.y);
    }

     private void MoveAttack(){
        //stateMachine.Horizontal = impact.x;
        groundstateMachine.BossManager.RB.velocity = impact;
    }

     private void setDamageWeapon(){
        // this is for the sword therefore we can use it to handle the damage and the knockback
        if(groundstateMachine.SwordEnemies.TryGetComponent<SwordEnemies>(out SwordEnemies sword)){
            
            //Debug.Log("this is the knockback" + attack.Knockback);
            sword.SetAttack(attack.Damage,attack.Knockback,attack.isNotBlockable);
        }

          //stateMachine.Weapon.SetAttack(attack.Damage, attack.Knockback);
    
    }

}
