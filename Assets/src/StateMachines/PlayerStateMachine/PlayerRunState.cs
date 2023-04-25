using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    
    
    private readonly int NormalStateBlendHash = Animator.StringToHash("NormalBlendTree"); 
    private readonly int NormalBlendSpeedHash = Animator.StringToHash("SpeedNormalBlend"); 

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;
    
    
    
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        SoundManager.PlaySound("run");   
        //Debug.Log("Run State");  
        stateMachine.Animator.CrossFadeInFixedTime(NormalStateBlendHash, CrossFadeDuration); 
        //subscribe to event actions
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.RollEvent += OnRoll;
        //only when we are on the ground we want the air attack bool to be available but we only change state on the fall or jump state
        stateMachine.IsAirAttack = true;        
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        stateMachine.RB.velocity = new Vector2(stateMachine.Horizontal * stateMachine.PlayerSpeed, stateMachine.RB.velocity.y);
        
    }

    public override void Tick(float deltaTime)
    {
       
        //stateMachine.horizontal = Input.GetAxisRaw("Horizontal");
        stateMachine.Horizontal = stateMachine.InputReader.movementValueInputReader.x;
        
        if(CanGuard()){
            OnGuard();
        } 
        
         stateMachine.Animator.SetFloat(NormalBlendSpeedHash, 1 , AnimatorDampTime, deltaTime);

        if(stateMachine.InputReader.isAttacking){
            stateMachine.SwitchState(new PlayerAttackState(stateMachine,0));
            //stateMachine.RB.velocity = Vector2.zero;
            return;
         }
        
         
         if(stateMachine.Horizontal == 0f){
             stateMachine.Animator.SetFloat(NormalBlendSpeedHash, 0 , AnimatorDampTime, deltaTime);
            //SoundManager.AudioForWalk.Stop();
            return;
         }
         
       
        //  if(!SoundManager.AudioForWalk.isPlaying){
               
        //         SoundManager.PlaySound("run");   
        //  }
         

               

        
    }
    public override void Exit()
    {  //unsubscribe
        
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.RollEvent -= OnRoll;


    }



    private void OnJump(){
        if(stateMachine.IsGrounded()){
            stateMachine.SwitchState(new PlayerJumpState(stateMachine));
        }
    }
}
