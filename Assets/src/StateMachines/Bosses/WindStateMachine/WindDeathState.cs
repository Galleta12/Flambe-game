using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindDeathState : WindBaseState
{
    
    private readonly int ImpactHash = Animator.StringToHash("death");
    private const float CrossFadeDuration = 0.1f;
    
    private float delayTime = 10.0f;
    public WindDeathState(WindStateMachine windstateMachine) : base(windstateMachine)
    {
    }

    public override void Enter()
    {
        
       
        windstateMachine.BossManager.Animator.CrossFadeInFixedTime(ImpactHash,CrossFadeDuration);
        SoundManager.PlaySound("windDeath");
        GameObject.Destroy(windstateMachine.BossManager.gameObject, delayTime);
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        windstateMachine.BossManager.RB.velocity = Vector2.zero;
    }

    public override void Tick(float deltaTime)
    {
        
       

    }
    public override void Exit()
    {
        
    }

}
