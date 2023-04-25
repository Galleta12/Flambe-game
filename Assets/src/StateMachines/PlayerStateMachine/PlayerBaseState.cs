using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    // Start is called before the first frame update
    
    protected PlayerStateMachine stateMachine;



    public PlayerBaseState(PlayerStateMachine stateMachine){
        this.stateMachine = stateMachine;
    }


    
    protected void OnRoll(){
        if(stateMachine.CoolDownTimeRoll <=0f){
            stateMachine.SwitchState(new PlayerRollState(stateMachine, stateMachine.Horizontal));
        }
    }


    protected void handleCoolDownRoll(float deltaTime){

    //Debug.Log("The roll delegate is being call");
    // we reduce the time of the cooldown time frame by frame
    
    stateMachine.CoolDownTimeRoll -=deltaTime;
    if(stateMachine.CoolDownTimeRoll <=0f){
         // the delegate is only being call if it exists and it will only exist if we exit the state
        
        //Debug.Log("The roll delegate should be deleted");
        stateMachine.setCoolDownRoll -=handleCoolDownRoll;
        
        }
    }



    protected void CreateRollDust(){
        
            float dirParticle = stateMachine.isFacingRight?1:-1f;
            Vector3 rotationParticle = new Vector3();
            if(dirParticle == -1){
                rotationParticle = new Vector3(stateMachine.RollDust.transform.localRotation.eulerAngles.x,180f,stateMachine.RollDust.transform.localRotation.eulerAngles.z);
            }else{
                rotationParticle = new Vector3(stateMachine.RollDust.transform.localRotation.eulerAngles.x,0f,stateMachine.RollDust.transform.localRotation.eulerAngles.z);
            }
            stateMachine.RollDust.transform.localRotation = Quaternion.Euler(rotationParticle);
            stateMachine.RollDust.Play();
        
    }


    
    protected bool CanGuard(){
        if(stateMachine.InputReader.isGuarding && !stateMachine.Guard.isBroke){
            return true;
        }else{
            return false;
        }
    }
    
    protected void OnGuard(){
        //this only possible if you are on the ground
        stateMachine.SwitchState(new PlayerGuardState(stateMachine));
    }



 
}
