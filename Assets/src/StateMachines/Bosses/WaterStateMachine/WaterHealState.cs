using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHealState : WaterBaseState
{
    private readonly int HealHash = Animator.StringToHash("heal");

    private const float CrossFadeDuration = 0.1f;
    
    public WaterHealState(WaterStateMachine waterstateMachine) : base(waterstateMachine)
    {
    }

    public override void Enter()
    {
        //set the timer in the enter and exit to avoid errors
        SoundManager.PlaySound("healing");       
        waterstateMachine.SetTimeForHeal = waterstateMachine.HowLongBeforeHeal;
        waterstateMachine.IsHealing = true;
       
        waterstateMachine.transform.position = waterstateMachine.StartPosition;
        waterstateMachine.BossManager.Animator.CrossFadeInFixedTime(HealHash, CrossFadeDuration);
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        //We don't want to move when we are doing an attack on air
        waterstateMachine.BossManager.RB.velocity = Vector2.zero;
        //apply force equial to the gravity to cancel the gravity when the player is on the air
        Vector2 v = Physics2D.gravity * waterstateMachine.BossManager.RB.mass;
        //waterstateMachine.BossManager.RB.velocity = -v;
        waterstateMachine.BossManager.RB.AddForce(-v * 10f,ForceMode2D.Force);
    }

    public override void Tick(float deltaTime)
    {
        if(GetNormalizedTimeForAll(waterstateMachine.BossManager.Animator, "Heal") >=1.0f){
            waterstateMachine.SwitchState(new WaterIdleState(waterstateMachine));
        }
    }
    public override void Exit()
    {
        //start the timer again
        waterstateMachine.IsHealing = false;
        waterstateMachine.SetTimeForHeal = waterstateMachine.HowLongBeforeHeal;
        //increase the health
        //increase by 10 for now
        waterstateMachine.BossManager.Health.setHealth(20);

    }

}
