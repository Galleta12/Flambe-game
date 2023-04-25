using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpecialAttackState1 : WindSpecialAttackState
{
    
    private readonly int SPHash = Animator.StringToHash("sp_atk1");
    private const float CrossFadeDuration = 0.1f;
    private float remaingSpecialAttackTime = 2f;

    private Vector2 currentPlayerPos;
    public WindSpecialAttackState1(WindStateMachine windstateMachine) : base(windstateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("Special Attack 1");

        
        currentPlayerPos = windstateMachine.BossManager.PlayerGameObjct.transform.position;
        windstateMachine.BossManager.Animator.CrossFadeInFixedTime(SPHash,CrossFadeDuration);
         //windstateMachine.BossManager.Animator.Play(SPHash);
           windstateMachine.BossManager.Health.setInvunerable(true);
         windstateMachine.BossManager.PlayerCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =true;
        windstateMachine.BossManager.MyColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =true;

    }

    public override void FixedTick(float fixeddeltaTime)
    {
        base.FixedTick(fixeddeltaTime);
    }

    
    public override void Tick(float deltaTime)
    {
        //base.Tick(deltaTime);
        remaingSpecialAttackTime -= deltaTime;
         
        //Debug.Log("Outside change : " + remaingSpecialAttackTime);
           if(GetNormalizedTime(windstateMachine.BossManager.Animator) >= 1.0f){
                windstateMachine.transform.position = windstateMachine.StartPosition;
           }

        
        if(remaingSpecialAttackTime <=0){
            //Debug.Log("Inside change");
            
            windstateMachine.InstianteDangerZone(currentPlayerPos);
            //
            windstateMachine.transform.position = windstateMachine.StartPosition;
            windstateMachine.SwitchState(new WindSpecialAttackState2(windstateMachine,currentPlayerPos));
            return;
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        windstateMachine.BossManager.PlayerCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        windstateMachine.BossManager.MyColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        windstateMachine.BossManager.Health.setInvunerable(false);
    }


    
}
