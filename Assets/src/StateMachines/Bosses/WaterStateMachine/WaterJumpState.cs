using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterJumpState : WaterBaseState
{
    
    private readonly int JumpHash = Animator.StringToHash("up");

    private const float CrossFadeDuration = 0.1f;

    private Vector2 dir;

    private bool isAirAttack;
    
    
    
    
    public WaterJumpState(WaterStateMachine waterstateMachine, bool _isAirAttack) : base(waterstateMachine)
    {
        //we want to move to the inverse of the dir in x
        this.dir = (waterstateMachine.BossManager.PlayerGameObjct.transform.position - waterstateMachine.transform.position);
        this.isAirAttack = _isAirAttack;
    }

    public override void Enter()
    {
        waterstateMachine.BossManager.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
        waterstateMachine.BossManager.RB.velocity = Vector2.up * waterstateMachine.BossManager.JumpSpeed;
        if(isAirAttack){
              waterstateMachine.UIAirAttack.SetActive(true);
        }
    }
    public override void FixedTick(float fixeddeltaTime)
    {
        waterstateMachine.BossManager.RB.velocity = new Vector2(-dir.x,waterstateMachine.BossManager.RB.velocity.y);
    }

    public override void Tick(float deltaTime)
    {
      waterstateMachine.BossManager.FacePlayer();
      if(waterstateMachine.BossManager.RB.velocity.y <=0){
        
        if(isAirAttack){
            waterstateMachine.SwitchState(new WaterAirAttackState(waterstateMachine));
            return;
        }
        OnRoll();
        return;

      }  
    }

    public override void Exit()
    {
        waterstateMachine.UIAirAttack.SetActive(false);
    }

}
