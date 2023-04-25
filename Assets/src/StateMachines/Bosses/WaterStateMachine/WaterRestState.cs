using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRestState : WaterBaseState
{
    private readonly int IdleHash = Animator.StringToHash("idle"); 
    private const float CrossFadeDuration = 0.1f;

    private float threshold;
    
    private bool shouldSpecial;
    private bool shouldWaterAttackSpecial;

    public WaterRestState(WaterStateMachine waterstateMachine) : base(waterstateMachine)
    {
    }

    public override void Enter()
    {
        //waterstateMachine.BossManager.Animator.Play(IdleHash);
        threshold = waterstateMachine.BossManager.HowLongBeforePerformingAttack;
        waterstateMachine.BossManager.Animator.CrossFadeInFixedTime(IdleHash,CrossFadeDuration);
        waterstateMachine.BossManager.RB.velocity = Vector2.zero;
        //select what attack we want to do
        if(SpecialWaterAttackChance()){
            shouldWaterAttackSpecial = true;
            //waterstateMachine.UISpecialAttack.SetActive(true);
            return;
        }
        if(SpecialAttackChance()){
            shouldSpecial = true;
            waterstateMachine.UISpecialAttack.SetActive(true);
        }else{
            shouldSpecial = false;
            shouldWaterAttackSpecial = false;
            waterstateMachine.UIAttackCombo.SetActive(true);
        }
       
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        waterstateMachine.BossManager.RB.velocity = Vector2.zero;
    }

    public override void Tick(float deltaTime)
    {
        threshold -= deltaTime;
        
        
        if(shouldWaterAttackSpecial){
            waterstateMachine.SwitchState(new WaterRestSpecialAttackState(waterstateMachine));
            return;
        }
        
        if(threshold <= 0){
            
            if(shouldSpecial == true){
                waterstateMachine.SwitchState(new WaterAttackState(waterstateMachine,2));
            }else{
                waterstateMachine.SwitchState(new WaterAttackState(waterstateMachine,0));
            }
        }
    }
    public override void Exit()
    {
        waterstateMachine.UISpecialAttack.SetActive(false);
        waterstateMachine.UIAttackCombo.SetActive(false);

    }
}
