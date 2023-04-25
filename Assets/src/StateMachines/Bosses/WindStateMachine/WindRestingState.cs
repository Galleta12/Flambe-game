using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRestingState : WindBaseState
{
    
    
    private readonly int IdleHash = Animator.StringToHash("Resting"); 
    private const float CrossFadeDuration = 0.1f;

    private float threshold;
    
    private bool shouldSpecial;
    
    public WindRestingState(WindStateMachine windstateMachine) : base(windstateMachine)
    {
    }

    public override void Enter()
    {
        //windstateMachine.BossManager.Animator.Play(IdleHash);
        threshold = windstateMachine.BossManager.HowLongBeforePerformingAttack;
        windstateMachine.BossManager.Animator.CrossFadeInFixedTime(IdleHash,CrossFadeDuration);
        windstateMachine.BossManager.RB.velocity = Vector2.zero;
        //select what attack we want to do
        if(SpecialAttackChance()){
            shouldSpecial = true;
            windstateMachine.UISpecialAttack.SetActive(true);
        }else{
            shouldSpecial = false;
            windstateMachine.UIAttackCombo.SetActive(true);
        }
       
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        windstateMachine.BossManager.RB.velocity = Vector2.zero;
    }

    public override void Tick(float deltaTime)
    {
        threshold -= deltaTime;
        
        if(threshold <= 0){

            if(shouldSpecial == true){
                windstateMachine.SwitchState(new WindAttackingState(windstateMachine,2));
            }else{
                windstateMachine.SwitchState(new WindAttackingState(windstateMachine,0));
            }
        }
    }
    public override void Exit()
    {
        windstateMachine.UISpecialAttack.SetActive(false);
        windstateMachine.UIAttackCombo.SetActive(false);

    }

}
