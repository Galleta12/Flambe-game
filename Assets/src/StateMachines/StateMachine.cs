using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    
    private State currentState;

    public State CurrentState {get{return currentState;}}

    public void SwitchState(State newState){
        currentState?.Exit();
        currentState = newState;
        newState?.Enter();
    }

    
    private void FixedUpdate() {
        CustomFixedUpdate(Time.fixedDeltaTime);
        currentState?.FixedTick(Time.fixedDeltaTime);
    }

    private void Update() {
        CustomUpdate(Time.deltaTime);
        currentState?.Tick(Time.deltaTime);    
    }

    //create virtual functions that will be used inside the stateMachine 

    public virtual void CustomFixedUpdate(float fixedDeltatime){
        
    }

   public virtual void CustomUpdate(float deltatime){

   }
}
