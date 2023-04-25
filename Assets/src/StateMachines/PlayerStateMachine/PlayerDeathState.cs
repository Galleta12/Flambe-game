using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    
    
    private readonly int DeathHash = Animator.StringToHash("Death");
    private const float CrossFadeDuration = 0.1f;
    
    //private float delayTime = 5.0f;
    
    
    public PlayerDeathState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(DeathHash,CrossFadeDuration);
        //GameObject.Destroy(stateMachine.gameObject, delayTime);
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        stateMachine.RB.velocity = Vector2.zero;
    }

    public override void Tick(float deltaTime)
    {
    }
    public override void Exit()
    {
    }

}
