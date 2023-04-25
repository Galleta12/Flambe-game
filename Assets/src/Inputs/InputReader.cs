using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    
    
     private Controls controls;
     public bool  isJumping {get; private set;}

    public bool  isAttacking {get; private set;}

    public bool  isGuarding {get; private set;}


    public Vector2 movementValueInputReader {get; private set;}
    
    public event Action JumpEvent;

    public event Action RollEvent;

    
    
    
    
    private  void OnEnable()
    {
        //enable it
        // store instance of class controls
        controls = new Controls();
        // reference to this class
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
        
    }


    void OnDisable()
    {
        controls.Player.Disable();
    }
    
    private void Start(){
        
        
    }
    
    
    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed){
            isAttacking = true;
        }else if(!context.performed){
            isAttacking =false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // if(context.performed){
        //     isJumping = true;
        // }else if(!context.performed){
        //    isJumping = false;
        // }
         if(!context.performed){return;}
            JumpEvent?.Invoke();

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementValueInputReader = context.ReadValue<Vector2>();
       // Debug.Log(movementValueInputReader);

    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        if(!context.performed){return;}
        RollEvent?.Invoke();
    }

    public void OnGuard(InputAction.CallbackContext context)
    {
        if(context.performed){
            isGuarding = true;
        }else if(!context.performed){
            isGuarding =false;
        }
    }
}
