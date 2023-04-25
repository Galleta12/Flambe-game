using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRollState : WindBaseState
{
    
    private readonly int RollHash = Animator.StringToHash("roll");
    private const float CrossFadeDuration = 0.1f;
    private float rollDir;

    private float remaingRollTime;
    
    private bool isInAir;
    public WindRollState(WindStateMachine windstateMachine, float rollDirection) : base(windstateMachine)
    {
        this.rollDir = rollDirection;
        isInAir = !windstateMachine.BossManager.IsGrounded();
       
        
    }

    public override void Enter()
    {
        remaingRollTime = windstateMachine.RollTime;
        windstateMachine.BossManager.Animator.CrossFadeInFixedTime(RollHash,CrossFadeDuration);
        //desactivethe collider and activate it again
        windstateMachine.BossManager.Health.setInvunerable(true);
         windstateMachine.BossManager.PlayerCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =true;
        windstateMachine.BossManager.MyColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =true;
      

        

    }
    public override void FixedTick(float fixeddeltaTime)
    {
        float horintalMove = rollDir * windstateMachine.RollSpeedMultiplayer/ windstateMachine.RollTime;
        
       if(!isInAir){
            windstateMachine.BossManager.RB.velocity = new Vector2(horintalMove,windstateMachine.transform.position.y);
       }else{
            windstateMachine.BossManager.RB.velocity = new Vector2(horintalMove,Vector2.up.y * 3f);
       }
    }

    public override void Tick(float deltaTime)
    {
        
        //for performance issue is better to don't use
        //windstateMachine.BossManager.ShadowEffect.ShadowSkill();
        
        remaingRollTime -= deltaTime;
        if(remaingRollTime <=0){
            if(!windstateMachine.BossManager.IsGrounded()){
                windstateMachine.SwitchState(new WindFallState(windstateMachine));
                return;
            }
            windstateMachine.SwitchState(new WindIdleState(windstateMachine));
        }
    }

    public override void Exit()
    {
        
        windstateMachine.BossManager.PlayerCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        windstateMachine.BossManager.MyColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        windstateMachine.BossManager.Health.setInvunerable(false);
        
   

    }

}
