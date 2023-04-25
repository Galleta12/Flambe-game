using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    
    
    
    
    private readonly int RollHash = Animator.StringToHash("Roll");
    private const float CrossFadeDuration = 0.1f;
    private float rollDir;

    private float remaingRollTime;



    //private bool isInAir;
    public PlayerRollState(PlayerStateMachine stateMachine, float rollDirection) : base(stateMachine)
    {
        
        
        stateMachine.IsRollAir =  !stateMachine.IsGrounded()?true:false;
        
        if(rollDirection == 0f){
            
            this.rollDir = stateMachine.isFacingRight? 1f:-1f;
        }else{
            this.rollDir = rollDirection;

        }
    }

    public override void Enter()
    {
        stateMachine.IsRolling = true;
        //desactive the character blocker collision to avoid any collision during the roll
        
        
        SoundManager.PlaySound("roll");
        remaingRollTime = stateMachine.RollTime;
        stateMachine.Animator.CrossFadeInFixedTime(RollHash,CrossFadeDuration);
        CreateRollDust();
        
        stateMachine.Health.setInvunerable(true);
        stateMachine.ColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =true;
        if(stateMachine.EnemyReference != null && stateMachine.EnemyReference.activeSelf){
            stateMachine.EnemyCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =true;
        }
}

    public override void FixedTick(float fixeddeltaTime)
    {
         float horizontalMove = rollDir * stateMachine.RollSpeedMultiplayer / stateMachine.RollTime;
        if(!stateMachine.IsRollAir){
            stateMachine.RB.velocity = new Vector2(horizontalMove,stateMachine.transform.position.y);
            //Debug.Log("Not roll in air");
       }else{
            stateMachine.RB.velocity = new Vector2(horizontalMove,Vector2.up.y * 3f);
            //Debug.Log("Roll in air");
       }    
    }

    public override void Tick(float deltaTime)
    {
        
        stateMachine.ShadowEffect.ShadowSkill();
        remaingRollTime -= deltaTime;
        
       
        //Debug.Log(stateMachine.Horizontal);

        if(remaingRollTime <=0f){
            if(!stateMachine.IsGrounded()){
                stateMachine.SwitchState(new PlayerFallState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new PlayerRunState(stateMachine));
        }
    }
    public override void Exit()
    {
        
        
        //we set the time to the roll cool down 
        stateMachine.CoolDownTimeRoll = stateMachine.RollCoolDown;
        stateMachine.setCoolDownRoll += handleCoolDownRoll;
        //affect gravity
        stateMachine.IsRollAir = false;
        
        if(stateMachine.EnemyReference != null && stateMachine.EnemyReference.activeSelf){
            stateMachine.EnemyCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        }

        stateMachine.ColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        stateMachine.Health.setInvunerable(false);
        stateMachine.IsRolling = false;

    }

}
