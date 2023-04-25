using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindStateMachine : StateMachine
{
    
    [field:SerializeField] public BossManager BossManager {get;private set;}

    [field:SerializeField] public SwordEnemies SwordEnemies {get;private set;}


    [field:SerializeField] public GuardStamina Guard {get;private set;}

    [field: SerializeField] public AirAttackData AirAttack {get; private set; }

    [field:SerializeField] public CollisionRangeCollider ColliderRange {get; private set; }
    [field:SerializeField] public GameObject SlashObject {get; private set; }

    [field:SerializeField] public GameObject SlashShooterPoint {get; private set; }
    [field:SerializeField] public GameObject UIAttackCombo {get;private set;}

    [field:SerializeField] public GameObject UISpecialAttack {get;private set;}

    [field:SerializeField] public GameObject UIAirAttack {get;private set;}

    [field:SerializeField] public GameObject DangerZone {get;private set;}

    //[field: SerializeField] public float HowLongBroke {get; private set; }
    
    
     [field: SerializeField] public float HowLongBeforeRunning {get; private set; }

    [field: SerializeField] public float RollSpeedMultiplayer {get; private set; }
    // how long the dash will took
    [field: SerializeField] public float RollTime {get; private set; }

    [field: SerializeField] public float LikeHoodAirAttack {get; private set;}

    [field: SerializeField] public float LikeHoodSpecialGroundAttack {get; private set;}


    //must be big number
    [field: SerializeField] public float IntervalsPerSuperSpecialAttack {get; private set;}

    [field: SerializeField] public int DamageSpecialAttack {get; private set;}


    [field: SerializeField] public float KnockBackSpecialAttack {get; private set;}

    [field: SerializeField] public bool SecondPhaseStart {get; private set;}

     [field: SerializeField] public int HealthForSecondPhase {get; private set;}

   
    public Color colorSprite {get;private set;}

   

    public Vector2 vecGravity {get; private set; }
    
    private bool isDefending;

    public bool IsDefending{get{return isDefending;}set{isDefending = value;}}

    private bool isairAttack;

    public bool IsAirAttack{get{return isairAttack;}set{isairAttack = value;}}

    
    private bool canAttack;

    public bool CanAttack{get{return canAttack;}set{canAttack=value;}}

   //this is for the special attacl
    public Vector3 StartPosition{get;private set;}

    

    private void Start(){
      
        
        
        StartPosition = transform.position; 
        //start the gurad event
       
        //initialize values
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        //save color of sprite
        colorSprite = BossManager.Sprite.color;
        
      
        // we start on the 
        if(BossManager.IsGrounded()){
            SwitchState(new WindIdleState(this));
        }else{
            SwitchState(new WindFallState(this));
        }
    
    }

  

    private void OnEnable() {
      
        
        ColliderRange.OnSlashMovingToward += OnDodge;
        BossManager.Health.OnDeath +=HandleDeath;
        BossManager.Health.OnTakeDamage += HandleImpact;
        

    }


    private void OnDisable() {
       
    
        ColliderRange.OnSlashMovingToward -= OnDodge;
        BossManager.Health.OnDeath -=HandleDeath;
        BossManager.Health.OnTakeDamage -= HandleImpact;
        //stop all courotines
        StopAllCoroutines();
    }

    

    public override void CustomUpdate(float deltatime)
    {
        
        
        
        int currentHealth = BossManager.Health.GetHealth();
        
        int currentGuardStamina =  Guard.GetGuardStamina();
       

        if(Guard.isBroke){
            StartCoroutine(FlashRed());
        }
        // if(Input.GetKeyDown(KeyCode.Z)){
        //     SwitchState(new WindSpecialAttackState1(this));
        // }
        if(HealthForSecondPhase >= currentHealth && SecondPhaseStart == false){
            SecondPhaseStart = true;
            //SwitchState(new WindSpecialAttackState1(this));
            ChangeToSuperSpecialAttack();

        }

        
    }


      //start the attack and special ground attack
    private IEnumerator StartAttack(float delay){
        //ResetAttackFlags();
        UIAttackCombo.SetActive(true);
        SwitchState(new WindRestingState(this));
        yield return new WaitForSeconds(delay);
        UIAttackCombo.SetActive(false);
        SwitchState(new WindAttackingState(this,0));

    }

    private IEnumerator StartSpecialGroundAttack(float delay){
        //ResetAttackFlags();
        yield return new WaitForSeconds(delay);
        UISpecialAttack.SetActive(false);
        //index of the special attack
        SwitchState(new WindAttackingState(this,2));

    }
    private IEnumerator StartChaseState(float delay){
        yield return new WaitForSeconds(delay);
        SwitchState(new WindChaseState(this));

    }

    public void ResetAttackFlags(){
        canAttack =false;
      
    }

    public void SlashThrow(int damage, float knockback){
        
        
        //Vector3 directionToMove = (BossManager.PlayerGameObjct.transform.position - transform.position).normalized;

        GameObject slash = Instantiate(SlashObject,SlashShooterPoint.transform.position,SlashShooterPoint.transform.rotation);
        
        
        slash.GetComponent<SlashAttackEnemy>().SetAttack(damage,knockback,AirAttack.isNotBlockable);
            
            
        

    }



    // it will change to the death state everytime the OnDeath event is trigger
    private void HandleDeath(){
        SwitchState(new WindDeathState(this));
    }


     private void HandleImpact(Vector2 directionknockback){
        
        //if(!BossManager.PlayerMachine.IsAttacking && Guard.isBroke){

            //flash red
            StartCoroutine(FlashRed());
            if(!canAttack){
                SwitchState(new WindImpactState(this,directionknockback));
            }
        //}
    }

 
    
    private void OnDodge()
    {
        if(!Guard.isBroke){
            GetRollDir();
        }
    }

    private void GetRollDir()
    {
       
        float randomDir = BossManager.FacingLeft? -1:1;
        
        //Debug.Log("This is the dir roll: " + randomDir);
        SwitchState(new WindRollState(this,randomDir));
    }


    public void InstianteDangerZone(Vector2 positon){
        Instantiate(DangerZone,(Vector3)positon,Quaternion.identity);
        
    }


    public void DestroyDangerZone(){
        GameObject DangerZone = GameObject.FindGameObjectWithTag("DangerZone");
        GameObject.Destroy(DangerZone,0.1f);

    }



    private void ChangeToSuperSpecialAttack(){
        // if(SecondPhaseStart == true){
        //     SecondPhaseStart = false;
        // }
        if(BossManager.Health.GetHealth()!=0){

            SwitchState(new WindSpecialAttackState1(this));
        }

    }


    public void SetCourotine(){
        
        StopCoroutine("StartSuperSpecialAttack");
        StartCoroutine("StartSuperSpecialAttack",IntervalsPerSuperSpecialAttack);
    }

    private IEnumerator StartSuperSpecialAttack(float delay){
        while(true){
            yield return new WaitForSeconds(delay);
            ChangeToSuperSpecialAttack();
        }
   }

   


    private IEnumerator FlashRed()
    {
      
        BossManager.Sprite.color = Color.red;

      
        yield return new WaitForSeconds(0.1f);
     
        BossManager.Sprite.color = colorSprite;

      
    }
}
