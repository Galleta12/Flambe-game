using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindBaseState : State
{
    protected WindStateMachine windstateMachine;

    public WindBaseState(WindStateMachine windstateMachine){
        this.windstateMachine = windstateMachine;
    }


    protected bool SpecialAttackChance(){
        int specialAttackChance = Random.Range(0,100);
        if(specialAttackChance <= windstateMachine.LikeHoodSpecialGroundAttack){
           // windstateMachine.CanAttackSpecialGround =true;
           return true;
        }else{
            //windstateMachine.CanAttackSpecialGround = false;
            //sience we can't special attack we can start doing the normal attack combo
            return false;
        }
    }


    protected bool AirAttackChance(){
        int specialAttackChance = Random.Range(0,100);
        if(specialAttackChance <= windstateMachine.LikeHoodAirAttack){
           // windstateMachine.CanAttackSpecialGround =true;
           return true;
        }else{
            //windstateMachine.CanAttackSpecialGround = false;
            //sience we can't special attack we can start doing the normal attack combo
            return false;
        }
    }


    

    protected bool AttackContinue(){
        int continueAttackChance = Random.Range(0,100);
        if(continueAttackChance <= windstateMachine.BossManager.LikeHoodContinueAttacking){
            //windstateMachine.CanContinueAttacking = true;
            return true;
        }else{
            //windstateMachine.CanContinueAttacking = false;
            return false;
            //sience we can't special attack we can start doing the normal attack combo
        }
    }

    protected void OnRoll(){
        
        int randomDir =0;
        if(windstateMachine.BossManager.IsGrounded()){
            randomDir = (int)Mathf.Sign(Random.Range(-1,1));
        }else{
            randomDir = windstateMachine.BossManager.FacingLeft? -1:1;
        }
        //Debug.Log("This is the dir roll: " + randomDir);
        windstateMachine.SwitchState(new WindRollState(windstateMachine,randomDir));
        
    }

    protected void OnRollAirAttack(){
        
        int randomDir =0;
    
        //Inverse
        randomDir = windstateMachine.BossManager.FacingLeft? 1:-1;
        
        //Debug.Log("This is the dir roll: " + randomDir);
        windstateMachine.SwitchState(new WindRollState(windstateMachine,randomDir));
        
    }


    

    protected void CanDefend(){
        //check function to compare the likehood of roll and guard and check if you can guard or if you should roll
        
        if(windstateMachine.BossManager.PlayerMachine.IsAttacking && windstateMachine.BossManager.IsPlayerFacingEnemy() && windstateMachine.BossManager.IsPlayerInRange()){
          if(!windstateMachine.Guard.isBroke){
             //windstateMachine.OnGuarding?.Invoke();
             ShouldRollOrGuard();
          }
        }
    }

    
    private void ShouldRollOrGuard()
    {
        float rollProbability = windstateMachine.BossManager.LikeHoodRoll/ 100f;
        float guardProbability = windstateMachine.BossManager.LikeHoodGuard/ 100f;

        //generate random number between 0 and 1
        float randomValue = Random.value;
        if(randomValue < rollProbability){
            OnRoll();
        }
        //we want to take into account the two values 
        else if(randomValue < guardProbability + rollProbability ){
            ChangeGuardState();
        }
        //do nothing 
        else{
            //windstateMachine.SwitchState(new WindIdleState(windstateMachine));
            return;
        
        }

    }

    private void ChangeGuardState()
    {
        //Debug.Log("New block state");
        windstateMachine.SwitchState(new WindBlockState(windstateMachine));
    }
    



      
}
