using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDeathState : GroundBaseState
{
    
     private readonly int ImpactHash = Animator.StringToHash("death");
    private const float CrossFadeDuration = 0.1f;
    
    private float delayTime = 5.0f;
    
    
    
    public GroundDeathState(GroundStateMachine groundstateMachine) : base(groundstateMachine)
    {
    }

     public override void Enter()
    {
        groundstateMachine.BossManager.Animator.CrossFadeInFixedTime(ImpactHash,CrossFadeDuration);
        //SoundManager.PlaySound("windDeath");
        GameObject.Destroy(groundstateMachine.BossManager.gameObject, delayTime);
    }
    public override void FixedTick(float fixeddeltaTime)
    {
        groundstateMachine.BossManager.RB.velocity = Vector2.zero;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }

}
