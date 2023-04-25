using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRollState : WaterBaseState
{
    
    
    private readonly int RollHash = Animator.StringToHash("tumble");
    private const float CrossFadeDuration = 0.1f;
    private float rollDir;

    private float remaingRollTime;
    
    private bool isInAir;
    
    
    public WaterRollState(WaterStateMachine waterstateMachine,float rollDirection) : base(waterstateMachine)
    {
        this.rollDir = rollDirection;
        isInAir = !waterstateMachine.BossManager.IsGrounded();
    }

    public override void Enter()
    {
        remaingRollTime = waterstateMachine.RollTime;
        waterstateMachine.BossManager.Animator.CrossFadeInFixedTime(RollHash,CrossFadeDuration);
        //desactivethe collider and activate it again
        SetInvunerabilities();
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        float horintalMove = rollDir * waterstateMachine.RollSpeedMultiplayer/ waterstateMachine.RollTime;
        
       if(!isInAir){
            waterstateMachine.BossManager.RB.velocity = new Vector2(horintalMove,waterstateMachine.transform.position.y);
       }else{
            waterstateMachine.BossManager.RB.velocity = new Vector2(horintalMove,Vector2.up.y * 3f);
       }
    }

    public override void Tick(float deltaTime)
    {
        remaingRollTime -= deltaTime;
        if(remaingRollTime <=0){
            if(!waterstateMachine.BossManager.IsGrounded()){
                waterstateMachine.SwitchState(new WaterFallState(waterstateMachine));
                return;
            }
            waterstateMachine.SwitchState(new WaterIdleState(waterstateMachine));
        }
    }
    public override void Exit()
    {
       UnSetInvunerabilities();
    }

}
