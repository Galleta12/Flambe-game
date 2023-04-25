using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDeathState : WaterBaseState
{
    
    
    
    
    private readonly int ImpactHash = Animator.StringToHash("death");
    private const float CrossFadeDuration = 0.1f;
    
    private float delayTime = 5.0f;
    
    
    public WaterDeathState(WaterStateMachine waterstateMachine) : base(waterstateMachine)
    {
    }

    public override void Enter()
    {
        waterstateMachine.BossManager.Animator.CrossFadeInFixedTime(ImpactHash,CrossFadeDuration);
        //SoundManager.PlaySound("windDeath");
        GameObject.Destroy(waterstateMachine.BossManager.gameObject, delayTime);
    }
    public override void FixedTick(float fixeddeltaTime)
    {
        waterstateMachine.BossManager.RB.velocity = Vector2.zero;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }

}
