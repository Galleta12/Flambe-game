using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WaterBaseState : State
{
    protected WaterStateMachine waterstateMachine;

    public WaterBaseState(WaterStateMachine waterstateMachine){
        this.waterstateMachine = waterstateMachine;
    }


    protected bool SpecialAttackChance(){
        int specialAttackChance = Random.Range(0,100);
        if(specialAttackChance <= waterstateMachine.LikeHoodSpecialGroundAttack){
           // waterstateMachine.CanAttackSpecialGround =true;
           return true;
        }else{
            //waterstateMachine.CanAttackSpecialGround = false;
            //sience we can't special attack we can start doing the normal attack combo
            return false;
        }
    }


    protected bool SpecialWaterAttackChance(){
        int specialWaterAttackChance = Random.Range(0,100);
        if(specialWaterAttackChance <= waterstateMachine.LikeHoodWaterSpecialAttack){
           // waterstateMachine.CanAttackSpecialGround =true;
           return true;
        }else{
            //waterstateMachine.CanAttackSpecialGround = false;
            //sience we can't special attack we can start doing the normal attack combo
            return false;
        }
    }




    protected bool AirAttackChance(){
        int specialAttackChance = Random.Range(0,100);
        if(specialAttackChance <= waterstateMachine.LikeHoodAirAttack){
           // waterstateMachine.CanAttackSpecialGround =true;
           return true;
        }else{
            //waterstateMachine.CanAttackSpecialGround = false;
            //sience we can't special attack we can start doing the normal attack combo
            return false;
        }
    }

    protected bool RollBeforeAttackChance(){
        int specialAttackChance = Random.Range(0,100);
        if(specialAttackChance <= waterstateMachine.LikeHoodRollBeforeAttack){
           return true;
        }else{
            return false;
        }
    }


    

    protected bool AttackContinue(){
        int continueAttackChance = Random.Range(0,100);
        if(continueAttackChance <= waterstateMachine.BossManager.LikeHoodContinueAttacking){
            //waterstateMachine.CanContinueAttacking = true;
            return true;
        }else{
            //waterstateMachine.CanContinueAttacking = false;
            return false;
            //sience we can't special attack we can start doing the normal attack combo
        }
    }
    protected void CanDefend(){
        //check function to compare the likehood of roll and guard and check if you can guard or if you should roll
        
        if(IsPlayerAttacking() && waterstateMachine.BossManager.IsPlayerFacingEnemy() 
        && waterstateMachine.BossManager.IsPlayerInRange()){
          if(!waterstateMachine.Guard.isBroke){
             ShouldRollOrGuard();
          }
        }
    }

    protected void OnRoll(){
        
        int randomDir =0;
        if(waterstateMachine.BossManager.IsGrounded()){
            randomDir = (int)Mathf.Sign(Random.Range(-1,1));
        }else{
            randomDir = waterstateMachine.BossManager.FacingLeft? -1:1;
        }
        //Debug.Log("This is the dir roll: " + randomDir);
        waterstateMachine.SwitchState(new WaterRollState(waterstateMachine,randomDir));
        
    }

    

    protected void OnRollAirAttack(){
        
        int randomDir =0;
    
        //Inverse
        randomDir = waterstateMachine.BossManager.FacingLeft? 1:-1;
        
        //Debug.Log("This is the dir roll: " + randomDir);
        waterstateMachine.SwitchState(new WaterRollState(waterstateMachine,randomDir));
        
    }


    


    
    private void ShouldRollOrGuard()
    {
        float rollProbability = waterstateMachine.BossManager.LikeHoodRoll/ 100f;
        float guardProbability = waterstateMachine.BossManager.LikeHoodGuard/ 100f;

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
            Debug.Log("Do nothing");
            //waterstateMachine.SwitchState(new WindIdleState(waterstateMachine));
            return;
        
        }

    }

    private void ChangeGuardState()
    {
        //Debug.Log("New block state");
        waterstateMachine.SwitchState(new WaterBlockState(waterstateMachine));
        return;
    }

    protected void SetInvunerabilities(){
        waterstateMachine.BossManager.Health.setInvunerable(true);
        waterstateMachine.BossManager.PlayerCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =true;
        waterstateMachine.BossManager.MyColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =true;
    }

    protected void UnSetInvunerabilities(){
        waterstateMachine.BossManager.PlayerCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        waterstateMachine.BossManager.MyColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        waterstateMachine.BossManager.Health.setInvunerable(false);  
    }

    protected bool IsPlayerJumping(){
        if(waterstateMachine.BossManager.PlayerMachine.IsJumping){
            return true;
        }else{
            return false;
        }
    }    
    protected bool IsPlayerAttacking(){
        if(waterstateMachine.BossManager.PlayerMachine.IsAttacking){
            return true;
        }else{
            return false;
        }
    }

    


}
