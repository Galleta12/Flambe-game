using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRestState : GroundBaseState
{
    
    private readonly int IdleHash = Animator.StringToHash("idle"); 
    private const float CrossFadeDuration = 0.1f;

    private float threshold;
    
    private bool shouldSpecial;
    
    
    public GroundRestState(GroundStateMachine groundstateMachine) : base(groundstateMachine)
    {
    }

    public override void Enter()
    {
        //groundstateMachine.BossManager.Animator.Play(IdleHash);
        threshold = groundstateMachine.BossManager.HowLongBeforePerformingAttack;
        groundstateMachine.BossManager.Animator.CrossFadeInFixedTime(IdleHash,CrossFadeDuration);
        groundstateMachine.BossManager.RB.velocity = Vector2.zero;
        //select what attack we want to do
        if(SpecialAttackChance()){
            shouldSpecial = true;
            groundstateMachine.UISpecialAttack.SetActive(true);
        }else{
            shouldSpecial = false;
            groundstateMachine.UIAttackCombo.SetActive(true);
        }
       
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        groundstateMachine.BossManager.RB.velocity = Vector2.zero;
    }

    public override void Tick(float deltaTime)
    {
        threshold -= deltaTime;
        
        if(threshold <= 0){

            if(shouldSpecial == true){
                groundstateMachine.SwitchState(new GroundAttackingState(groundstateMachine,2));
            }else{
                groundstateMachine.SwitchState(new GroundAttackingState(groundstateMachine,0));
            }
        }
    }
    public override void Exit()
    {
        groundstateMachine.UISpecialAttack.SetActive(false);
        groundstateMachine.UIAttackCombo.SetActive(false);

    }

    
}
