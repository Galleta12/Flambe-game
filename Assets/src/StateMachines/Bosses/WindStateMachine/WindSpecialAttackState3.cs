using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpecialAttackState3 : WindSpecialAttackState
{
    
    
    private readonly int SP3Hash = Animator.StringToHash("sp_atk3");
    private const float CrossFadeDuration = 0.1f;
    
    private Vector2 newPlayerPos;

    
    
    public WindSpecialAttackState3(WindStateMachine windstateMachine, Vector2 newPlayerPos) : base(windstateMachine)
    {
        this.newPlayerPos = newPlayerPos;
        windstateMachine.transform.position = newPlayerPos;
    }

    public override void Enter()
    {
        //base.Enter();
        
        //Debug.Log("Special Attack 3");
        windstateMachine.BossManager.PlayerCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        windstateMachine.BossManager.MyColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        windstateMachine.BossManager.Health.setInvunerable(false);
        windstateMachine.DestroyDangerZone();
        windstateMachine.BossManager.Animator.CrossFadeInFixedTime(SP3Hash,CrossFadeDuration);
        //windstateMachine.BossManager.Animator.Play(SP3Hash);
      
    }

    

    public override void FixedTick(float fixeddeltaTime)
    {
        base.FixedTick(fixeddeltaTime);
        
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
          
          if(GetNormalizedTime(windstateMachine.BossManager.Animator) >= 1.0f){
          
                //Debug.Log("THis is true");
                
                windstateMachine.transform.position = windstateMachine.StartPosition;
                windstateMachine.SwitchState(new WindIdleState(windstateMachine));

            
          }
          
    }
    public override void Exit()
    {
        //base.Exit();
        windstateMachine.BossManager.Health.setInvunerable(false);
        windstateMachine.BossManager.MyColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        windstateMachine.BossManager.PlayerCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        //exit we want to start the courotine
        windstateMachine.SetCourotine();
    }


    
}
