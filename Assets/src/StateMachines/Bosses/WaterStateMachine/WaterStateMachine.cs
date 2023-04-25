using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStateMachine : StateMachine
{
     [field:SerializeField] public BossManager BossManager {get;private set;}

    
    [field:SerializeField] public GameObject WaterAttackPrefab {get;private set;}
    
    [field:SerializeField] public GameObject SpecialAttackPos {get;private set;}
    
    [field:SerializeField] public GameObject[] SpecialAttacksWaterPos {get;private set;}
    
    
    [field:SerializeField] public SwordEnemies SwordEnemies {get;private set;}

    [field:SerializeField] public GuardStamina Guard {get;private set;}

    [field: SerializeField] public AirAttackData AirAttack {get; private set; }

    [field:SerializeField] public CollisionRangeCollider ColliderRange {get; private set; }
    [field:SerializeField] public GameObject SlashObject {get; private set; }

    [field:SerializeField] public GameObject SlashShooterPoint {get; private set; }
    [field:SerializeField] public GameObject UIAttackCombo {get;private set;}

    [field:SerializeField] public GameObject UISpecialAttack {get;private set;}

    [field:SerializeField] public GameObject UIAirAttack {get;private set;}

    //[field:SerializeField] public GameObject DangerZone {get;private set;}

    [field: SerializeField] public float HowLongBeforeRunning {get; private set; }

    [field: SerializeField] public float RollSpeedMultiplayer {get; private set; }
    // how long the dash will took
    [field: SerializeField] public float RollTime {get; private set; }

    [field: SerializeField] public float LikeHoodRollBeforeAttack {get; private set;}
    [field: SerializeField] public float LikeHoodAirAttack {get; private set;}

    [field: SerializeField] public float LikeHoodSpecialGroundAttack {get; private set;}

     [field: SerializeField] public float LikeHoodWaterSpecialAttack {get; private set;}
    [field: SerializeField] public float HowLongBeforeHeal {get; private set;}
    //must be big number

    [field: SerializeField] public bool SecondPhaseStart {get; private set;}
   

   
    public Color colorSprite {get;private set;}

    public Vector2 vecGravity {get; private set; }
    
    private bool isDefending;

    public bool IsDefending{get{return isDefending;}set{isDefending = value;}}

    private bool isairAttack;

    public bool IsAirAttack{get{return isairAttack;}set{isairAttack = value;}}

    
    private bool canAttack;

    public bool CanAttack{get{return canAttack;}set{canAttack=value;}}
    private bool isHealing;

    public bool IsHealing{get{return isHealing;}set{isHealing=value;}}
    private float setTimeForHeal = 0f;

    public float SetTimeForHeal{get{return setTimeForHeal;}set{setTimeForHeal = value;}}

   //this is for the special attacl
    public Vector3 StartPosition{get;private set;}

    private bool isDeath;

    //for the healing



    private void Start(){
      
        
        //set the timer at the star as well
        setTimeForHeal = HowLongBeforeHeal;
        
        StartPosition = transform.position; 
        //start the gurad event
       
        //initialize values
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        //save color of sprite
        colorSprite = BossManager.Sprite.color;
        
      
        // we start on 
        if(BossManager.IsGrounded()){
           SwitchState(new WaterIdleState(this));
        }else{
            SwitchState(new WaterFallState(this));
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
        //StopAllCoroutines();
    }

    public override void CustomUpdate(float deltatime)
    {
        
        //get the elapsed time
        if(!isHealing){

            setTimeForHeal -= deltatime;
        }

       // Debug.Log("This is the time for heal" + setTimeForHeal);
        int currentHealth = BossManager.Health.GetHealth();
       
        int currentGuardStamina =  Guard.GetGuardStamina();
       

        if(Guard.isBroke){
           //ColorBlueBrokeStamina();
            StartCoroutine(FlashRed());
            
        }
        // if(!Guard.isBroke){
        //     ColorNormal();
        // }
        // if(Input.GetKeyDown(KeyCode.Z)){
        //     SwitchState(new WaterRestSpecialAttackState(this));
        // }
    
        
        //only change to the heal state if the time is less than 0 and we are not currently healing
        // and we are not death
        if(setTimeForHeal <= 0f && !isHealing && !isDeath && !canAttack && 
        !(BossManager.Health.maxhealth == BossManager.Health.GetHealth())){
            //Debug.Log("Change to heal state");
            isHealing = true;
            SwitchState(new WaterHealState(this));
            return;
        }
        
    }

    private void HandleDeath(){
        isDeath = true;
        SwitchState(new WaterDeathState(this));
    }


     private void HandleImpact(Vector2 directionknockback){
        
           // Debug.Log("should change to impact state");
            
            //flash red
            StartCoroutine(FlashRed());
            if(!isHealing){
                setTimeForHeal = HowLongBeforeHeal;
            }
        
            if(!canAttack){
                SwitchState(new WaterImpactState(this,directionknockback));
            }
        
    }

    private IEnumerator FlashRed()
    {
      
        BossManager.Sprite.color = Color.red;

      
        yield return new WaitForSeconds(0.1f);
     
        BossManager.Sprite.color = colorSprite;

      
    }

    private void OnDodge()
    {
        if(!Guard.isBroke && !isHealing){
            GetRollDir();
        }
    }

    public void GetRollDir()
    {
       
        float dir = BossManager.FacingLeft? -1:1;
        
        //Debug.Log("This is the dir roll: " + dir);
        SwitchState(new WaterRollState(this,dir));
    }


    public void SlashThrow(int damage, float knockback){
        
        
        //Vector3 directionToMove = (BossManager.PlayerGameObjct.transform.position - transform.position).normalized;

        GameObject slash = Instantiate(SlashObject,SlashShooterPoint.transform.position,SlashShooterPoint.transform.rotation);
        
        
        slash.GetComponent<SlashAttackEnemy>().SetAttack(damage,knockback,AirAttack.isNotBlockable);
            
            
        

    }


    public void WaterThrow(){
        
        foreach (GameObject g in SpecialAttacksWaterPos){
            Instantiate(WaterAttackPrefab,g.transform.position,WaterAttackPrefab.transform.rotation);
        }
    }

       //change color
    private  void ColorBlueBrokeStamina()
    {
        if(BossManager.Sprite.color == Color.blue){return;}
        BossManager.Sprite.color = Color.blue;
      
    }

 
    private void ColorNormal()
    {
        if(BossManager.Sprite.color == colorSprite){return;}
        BossManager.Sprite.color = colorSprite;
    }
      

}
