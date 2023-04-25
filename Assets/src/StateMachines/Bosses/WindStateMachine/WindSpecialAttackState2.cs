using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpecialAttackState2 : WindSpecialAttackState
{
    
    private readonly int SP2Hash = Animator.StringToHash("sp_atk2");
    private const float CrossFadeDuration = 0.8f;
    
    
    private Vector2 playerPos;
    private float waintFewSeconds=3f;
    
    
    public WindSpecialAttackState2(WindStateMachine windstateMachine, Vector2 playerPos) : base(windstateMachine)
    {
        this.playerPos = playerPos;
        windstateMachine.transform.position = playerPos;
    }

    public override void Enter()
    {
        //base.Enter();
        //Debug.Log("Special Attack 2");
        
        
        
        windstateMachine.DestroyDangerZone();
        //we want to start a few distance above the player
       
        //windstateMachine.BossManager.Animator.CrossFadeInFixedTime(SP2Hash,CrossFadeDuration);
        windstateMachine.BossManager.Animator.Play(SP2Hash);
    }

    

    public override void FixedTick(float fixeddeltaTime)
    {
        base.FixedTick(fixeddeltaTime);
    }

    

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        
        
        waintFewSeconds -= deltaTime;
        
        //Debug.Log("Special Attack2  ");
        if(GetNormalizedTime(windstateMachine.BossManager.Animator) >= 1.0f){
            //Debug.Log("THis is true");
            windstateMachine.transform.position = windstateMachine.StartPosition;
            if(waintFewSeconds <=0){
                    Vector2 newPos= windstateMachine.BossManager.PlayerGameObjct.transform.position; 
                    windstateMachine.InstianteDangerZone(newPos);            
                    windstateMachine.SwitchState(new WindSpecialAttackState3(windstateMachine, newPos));
            }

        }
    }
    public override void Exit()
    {
        //base.Exit();
    }


    
}
