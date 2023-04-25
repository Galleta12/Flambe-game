using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpecialAttackState : WindBaseState
{
    
    
    
    

   
    
    
    
    public WindSpecialAttackState(WindStateMachine windstateMachine) : base(windstateMachine)
    {
    }

    public override void Enter()
    {
        windstateMachine.BossManager.Health.setInvunerable(true);
        windstateMachine.BossManager.PlayerCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =true;
        windstateMachine.BossManager.MyColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =true;
        setDamageWeapon();
      
    }

    public override void FixedTick(float fixeddeltaTime)
    {
        windstateMachine.BossManager.RB.velocity = Vector2.zero; 
    }

    public override void Tick(float deltaTime)
    {
    }
    public override void Exit()
    {
        windstateMachine.BossManager.Health.setInvunerable(false);
        windstateMachine.BossManager.MyColliderBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
        windstateMachine.BossManager.PlayerCharacterBlocker.GetComponent<CapsuleCollider2D>().isTrigger =false;
    }
     private void setDamageWeapon(){
        // this is for the sword therefore we can use it to handle the damage and the knockback
        if(windstateMachine.SwordEnemies.TryGetComponent<SwordEnemies>(out SwordEnemies sword)){
            sword.SetAttack(windstateMachine.DamageSpecialAttack,windstateMachine.KnockBackSpecialAttack,true);
        }

          //stateMachine.Weapon.SetAttack(attack.Damage, attack.Knockback);
    
    }

}
