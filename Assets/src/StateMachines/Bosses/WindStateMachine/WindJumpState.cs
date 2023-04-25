using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindJumpState : WindBaseState
{
    private readonly int JumpHash = Animator.StringToHash("up");

    private const float CrossFadeDuration = 0.1f;

    private Vector2 dir;

    private bool isAirAttack;

    public WindJumpState(WindStateMachine windstateMachine, bool _isAirAttack) : base(windstateMachine)
    {
      
      //we want to move to the inverse of the dir in x
      this.dir = (windstateMachine.BossManager.PlayerGameObjct.transform.position - windstateMachine.transform.position);
      this.isAirAttack = _isAirAttack;
      //Debug.Log(dir.x);
    }

    public override void Enter()
    {
        windstateMachine.BossManager.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
        windstateMachine.BossManager.RB.velocity = Vector2.up * windstateMachine.BossManager.JumpSpeed;

        //should jump front or behind
        if(isAirAttack){
          windstateMachine.UIAirAttack.SetActive(true);
        }

        
    }

    public override void Tick(float deltaTime)
    {
     windstateMachine.BossManager.FacePlayer();
      if(windstateMachine.BossManager.RB.velocity.y <=0){
        
        if(isAirAttack){
            windstateMachine.SwitchState(new WindAirAttack(windstateMachine));
            return;
        }
        OnRoll();

      }  
    }

    public override void FixedTick(float fixeddeltaTime)
    {
      //if(windstateMachine.IsAirAttack){
        //we want to move to the inverse  
        windstateMachine.BossManager.RB.velocity = new Vector2(-dir.x, windstateMachine.BossManager.RB.velocity.y);
      //}
    }

    public override void Exit()
    {
      windstateMachine.UIAirAttack.SetActive(false);
              
    }
}
