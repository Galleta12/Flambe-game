using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRestSpecialAttackState : WaterBaseState
{
    
    private readonly int SPHash = Animator.StringToHash("sp_atk1");

     private readonly int IdleHash = Animator.StringToHash("idle");

     private float waitSeconds = 2f;
    private bool isWaterThorw;

    public WaterRestSpecialAttackState(WaterStateMachine waterstateMachine) : base(waterstateMachine)
    {
        waterstateMachine.transform.position = waterstateMachine.SpecialAttackPos.transform.position;
    }

    public override void Enter()
    {
        waterstateMachine.CanAttack = true;
        waterstateMachine.BossManager.Animator.Play(SPHash);
        waterstateMachine.BossManager.FacePlayer();
        waterstateMachine.BossManager.Health.setInvunerable(true);
        waterstateMachine.UIAirAttack.SetActive(true);
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
        
        waterstateMachine.BossManager.FacePlayer();
        waitSeconds -= deltaTime;

        //Debug.Log(waitSeconds);
        if(GetNormalizedTimeForAll(waterstateMachine.BossManager.Animator, "SpecialAttack") >= 1.0f){
            if(!isWaterThorw){

                isWaterThorw = true;
                waterstateMachine.WaterThrow();
                //waterstateMachine.BossManager.Animator.Play(IdleHash);
            }
            if(waitSeconds <= 0){
                //Debug.Log("This should happen");
                waterstateMachine.SwitchState(new WaterFallState(waterstateMachine));
            }
        }

    }

    public override void Exit()
    {
        waterstateMachine.CanAttack = false;
        waterstateMachine.BossManager.Health.setInvunerable(false);
        waterstateMachine.UIAirAttack.SetActive(false);

    }

}
