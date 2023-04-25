using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStateMachine : StateMachine
{
    
        
    
    // Variable from the Inspector ------------------------------------------------------------
    
    
    
    [field:SerializeField] public InputReader InputReader {get; private set; }
    [field: SerializeField] public SwordDamage Sword {get; private set; }
    [field: SerializeField] public Health Health { get; private set; }

    [field: SerializeField] public GuardStamina Guard { get; private set; }
    [field: SerializeField] public Shadow ShadowEffect { get; private set; }
    [field: SerializeField] public Animator Animator {get; private set; }
    [field:SerializeField] public SpriteRenderer sprite {get; private set; }

    [field:SerializeField] public ParticleSystem Dust {get; private set; }

    [field:SerializeField] public ParticleSystem RollDust {get; private set; }
    [field:SerializeField] public Rigidbody2D RB {get; private set; }

    [field:SerializeField] public CapsuleCollider2D MyCollider {get; private set; }

    [field:SerializeField] public GameObject ColliderBlocker {get; private set; }

    [field:SerializeField] public Transform GroundCheck {get; private set; }

    [field:SerializeField] public GameObject SlashObject {get; private set; }

    [field:SerializeField] public GameObject SlashShooterPoint {get; private set; }

    [field:SerializeField] public GameObject  EnemyReference {get; private set; }

    [field:SerializeField] public GameObject EnemyCharacterBlocker {get; private set; }


    [field:SerializeField] public LayerMask GroundLayer {get; private set; }


    //Attack Data

    [field: SerializeField] public Attack[] Attacks {get; private set; }


    [field: SerializeField] public AirAttackData AirAttack {get; private set; }

    //Varaibles for Inspector ------------------------------------------------------------
     [field:SerializeField] public float PlayerSpeed {get; private set; }

    [field:SerializeField] public float JumpSpeed {get; private set; }

    
    [field:SerializeField] public float FallMultiplayer {get; private set; }

    // drag for external forces to the player, like the impact
    [field: SerializeField] public float Drag{get; private set; }


    [field: SerializeField] public float RollSpeedMultiplayer {get; private set; }
    // how long the dash will took
    [field: SerializeField] public float RollTime {get; private set; }
    // the cooldown for the dash
    [field: SerializeField] public float RollCoolDown {get; private set; }

    [field: SerializeField] public float HowLongImpact {get; private set; }

    


    //Varaible getter and setter easy, protective and clean way of accesing varaible from the context of the state and be able to modified the variable
    
    
    
    private float horizontal;
    public float Horizontal{get{return horizontal;}set{horizontal = value;}}


    
    private float coolDownTimeRoll =0;

    public float CoolDownTimeRoll{get{return coolDownTimeRoll;}set{coolDownTimeRoll=value;}}

  
    private bool isRollAir= false;

    public bool IsRollAir {get{return isRollAir;}set{isRollAir = value;}}



    private bool isRolling= false;

    public bool IsRolling {get{return isRolling;}set{isRolling = value;}}

    

    private bool isAttacking= false;

    public bool IsAttacking {get{return isAttacking;}set{isAttacking = value;}}

    private bool isImpact= false;

    public bool IsImpact {get{return isImpact;}set{isImpact = value;}}

    private bool isAirAttack = false;

    public bool IsAirAttack {get{return isAirAttack;}set{isAirAttack = value;}}

    private bool isAirAttackMove = false;

    public bool IsAirAttackMove {get{return isAirAttackMove;}set{isAirAttackMove = value;}}

    private bool isFalling = false;

    public bool IsFalling {get{return isFalling;}set{isFalling = value;}}

    private bool isJumping = false;

    public bool IsJumping {get{return isJumping;}set{isJumping = value;}}
    

    private bool isGuard = false;

    public bool IsGuard {get{return isGuard;}set{isGuard = value;}}



    private string currentAttackName;

    public string CurrentAttackName {get{return currentAttackName;}set{currentAttackName = value;}}



    // Variable not on the inspector this varaible you can't modified  ------------------------------------------------------------
    
    
    public Vector2 vecGravity {get; private set; }
    
     public Color colorSprite {get;private set;}

    public bool isFacingRight {get; private set;} = true;


    //delegates to set the cool down 

    public delegate void SetCoolDownRoll(float deltaTime);

    public SetCoolDownRoll setCoolDownRoll;



    // public GameObject EnemyReference {get;private set;}

    //  public GameObject EnemyCharacterBlocker {get;private set;}
    public Camera MainCamera {get; private set;}

    public Image rollCoolDownIndicator;
    

        
    
  
    
    private void updateRollIndicator(){
        rollCoolDownIndicator.fillAmount = (1f*coolDownTimeRoll);
    }
    
    private void OnEnable() {
        Health.OnDeath +=HandleDeath;
        Health.OnTakeDamage += HandleImpact;
    }


    private void OnDisable() {
       
        Health.OnDeath -=HandleDeath;
        Health.OnTakeDamage -= HandleImpact;

    }
    
    
    private void HandleImpact(Vector2 directionknockback)
    {
        StartCoroutine(FlashRed());
        SwitchState(new PlayerImpactState(this,directionknockback));
    }

    private void HandleDeath()
    {
        SwitchState(new PlayerDeathState(this));
    }
    
    
    
    
    
    
    
    private void Start() {
        
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        //save color of sprite
        colorSprite = sprite.color;
        //get reference to the collider blocker
        // EnemyReference = GameObject.FindGameObjectWithTag("Enemy");
        // EnemyCharacterBlocker = EnemyReference.transform.Find("CharacterCollisonBlocker").gameObject;
        MainCamera = Camera.main;
        
        if(!IsGrounded()){
            SwitchState(new PlayerFallState(this));
        }else{
            SwitchState(new PlayerRunState(this));
        }
    }
    
    



    public override void CustomFixedUpdate(float fixedDeltatime){
         
    }

    public override void CustomUpdate(float deltatime){
       
        //  if(Input.GetKeyDown(KeyCode.Space) && this.IsGrounded()){
           
        //     RB.velocity = Vector2.up * JumpSpeed;
        //  }
      
        int currentHealth = Health.GetHealth();
      
        int currentGuardStamina =  Guard.GetGuardStamina();
     
        
        Flip();
        setCoolDownRoll?.Invoke(deltatime);
        if(Guard.isBroke){
            StartCoroutine(FlashRed());
        }

        updateRollIndicator();

        //Debug.Log("Player state machine" + this.CurrentState);
        //not working
        // if(Guard.isBroke){
        //     FlashRed();
        //     return;
        // }
        

      //Debug.Log(IsGrounded());

    }
    
    
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }


    public void Flip(){
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f){
            CreateDust();
            isFacingRight = !isFacingRight;
            // Vector3 localScale = transform.localScale;
            // localScale.x *=-1f;
            // transform.localScale = localScale;
            transform.Rotate(0,180,0);
        }
    }



    public void SlashThrow(int damage, float knockback){
        float dirSlash = isFacingRight?1:-1f;
        Vector3 rotationSlash = new Vector3();
        if(dirSlash == -1f){

            rotationSlash = new Vector3(SlashShooterPoint.transform.localRotation.eulerAngles.x,180,
            SlashShooterPoint.transform.localRotation.eulerAngles.z);
        }

        if(isAirAttack){
            GameObject slash = Instantiate(SlashObject,SlashShooterPoint.transform.position,Quaternion.Euler(rotationSlash));
            slash.GetComponent<SlashAttack>().SetAttack(damage,knockback, DirectionToMouse().normalized);
            SoundManager.PlaySound("slash");
            
        }

    }

    public void CreateDust(){
        
        if(IsGrounded()){

            float dirParticle = isFacingRight?1:-1f;
            Vector3 rotationParticle = new Vector3();
            if(dirParticle == -1){
                rotationParticle = new Vector3(Dust.transform.localRotation.eulerAngles.x,180f,Dust.transform.localRotation.eulerAngles.z);
            }else{
                rotationParticle = new Vector3(Dust.transform.localRotation.eulerAngles.x,0f,Dust.transform.localRotation.eulerAngles.z);
            }
            Dust.transform.localRotation = Quaternion.Euler(rotationParticle);
            Dust.Play();
        }
    }

     private IEnumerator FlashRed()
    {
      
        sprite.color = Color.red;

      
        yield return new WaitForSeconds(0.1f);
     
        sprite.color = colorSprite;

      
    }


    private void OnTriggerEnter2D(Collider2D other) {
        
        
        Vector3 contact = other.transform.position;
        //avoid collision with itself
        if(other == MyCollider){return;}
        if(!other.CompareTag("Enemy")){return;}
        //Debug.Log(other.gameObject.name);
        //for now just jump. Maybe Create another bounce state for the player
        //this need more test
        if(!isRolling && !IsGrounded() && isFalling){
            SwitchState(new PlayerJumpState(this));
        }

    }


    
    public Vector2 DirectionToMouse(){
      Vector2 dirMouse = Input.mousePosition - MainCamera.WorldToScreenPoint(transform.position);      
      return dirMouse;

    }

    
    
    
    private void OnDrawGizmos() {
         
        // Gizmos.color = Color.black;
        // Gizmos.DrawLine(transform.position, (Vector2)transform.position + RB.velocity); 
        // Gizmos.color = Color.black;
        
        // // float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;   
        // Gizmos.DrawLine(transform.position,dir); 

    }
   

}
