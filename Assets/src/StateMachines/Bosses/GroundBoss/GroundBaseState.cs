using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundBaseState : State
{
    

    protected GroundStateMachine groundstateMachine;

    public GroundBaseState(GroundStateMachine groundstateMachine){
        this.groundstateMachine = groundstateMachine;
    }

    
    protected bool AttackContinue(){
        int continueAttackChance = Random.Range(0,100);
        if(continueAttackChance <= groundstateMachine.BossManager.LikeHoodContinueAttacking){
            //waterstateMachine.CanContinueAttacking = true;
            return true;
        }else{
            //waterstateMachine.CanContinueAttacking = false;
            return false;
            //sience we can't special attack we can start doing the normal attack combo
        }
    }
    
    
    protected bool SpecialAttackChance(){
        int specialAttackChance = Random.Range(0,100);
        if(specialAttackChance <= groundstateMachine.LikeHoodSpecialGroundAttack){
           // waterstateMachine.CanAttackSpecialGround =true;
           return true;
        }else{
            //waterstateMachine.CanAttackSpecialGround = false;
            //sience we can't special attack we can start doing the normal attack combo
            return false;
        }
    }
    
    
    
    protected void CanDefend(){
        //check function to compare the likehood of roll and guard and check if you can guard or if you should roll
        
        if(IsPlayerAttacking() && groundstateMachine.BossManager.IsPlayerFacingEnemy() 
        && groundstateMachine.BossManager.IsPlayerInRange()){
          if(!groundstateMachine.Guard.isBroke){
             ShouldGuard();
          }
        }
    }

    private void ShouldGuard()
    {
        int groundGuardChance = Random.Range(0,100);
        if(groundGuardChance <= groundstateMachine.BossManager.LikeHoodGuard){
           ChangeGuardState();
        }else{
            return;
        }
    }

    private void ChangeGuardState()
    {
        //Debug.Log("New block state");
        groundstateMachine.SwitchState(new GroundBlockState(groundstateMachine));
        return;
    }




    protected bool IsPlayerAttacking(){
        if(groundstateMachine.BossManager.PlayerMachine.IsAttacking){
            return true;
        }else{
            return false;
        }
    }

    protected bool IsPlayerJumping(){
        if(groundstateMachine.BossManager.PlayerMachine.IsJumping){
            return true;
        }else{
            return false;
        }
    }    



}
