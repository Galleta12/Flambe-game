using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHealingState : GroundBaseState
{
     private readonly int HealHash = Animator.StringToHash("meditate");

    private const float CrossFadeDuration = 0.1f;
    
    
    public GroundHealingState(GroundStateMachine groundstateMachine) : base(groundstateMachine)
    {
    }

     public override void Enter()
    {
        //set the timer in the enter and exit to avoid errors
        //groundstateMachine.ParticleHealing.Play();
        SoundManager.PlaySound("healing");       
        groundstateMachine.SetTimeForHeal = groundstateMachine.HowLongBeforeHeal;
        groundstateMachine.IsHealing = true;
        groundstateMachine.transform.position = groundstateMachine.StartPosition;
        groundstateMachine.BossManager.Animator.CrossFadeInFixedTime(HealHash, CrossFadeDuration);
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        //We don't want to move when we are doing an attack on air
        groundstateMachine.BossManager.RB.velocity = Vector2.zero;
        //apply force equial to the gravity to cancel the gravity when the player is on the air
        Vector2 v = Physics2D.gravity * groundstateMachine.BossManager.RB.mass;
        //groundstateMachine.BossManager.RB.velocity = -v;
        groundstateMachine.BossManager.RB.AddForce(-v * 10f,ForceMode2D.Force);
    }

    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTimeForAll(groundstateMachine.BossManager.Animator, "Heal") >=1.0f){
            groundstateMachine.SwitchState(new GroundIdleState(groundstateMachine));
        }
    }
    public override void Exit()
    {
        //start the timer again
        groundstateMachine.IsHealing = false;
        groundstateMachine.SetTimeForHeal = groundstateMachine.HowLongBeforeHeal;
        //increase the health
        //increase by 10 for now
        groundstateMachine.BossManager.Health.setHealth(20);
        //groundstateMachine.ParticleHealing.Stop();

    }
}
