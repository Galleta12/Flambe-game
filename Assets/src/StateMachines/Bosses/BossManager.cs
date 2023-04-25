using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager: MonoBehaviour
{
   
    [field: SerializeField] public Health Health {get; private set; }
    [field: SerializeField] public Animator Animator {get; private set; }
    [field:SerializeField] public SpriteRenderer Sprite {get; private set; }
    [field:SerializeField] public Rigidbody2D RB {get; private set; }
    [field:SerializeField] public CapsuleCollider2D MyCollider {get; private set; }

    [field:SerializeField] public CapsuleCollider2D MyColliderBlocker {get; private set; }

    [field:SerializeField] public Transform GroundCheck {get; private set; }

    [field:SerializeField] public LayerMask GroundLayer {get; private set; }

    [field:SerializeField] public LayerMask PlayerLayer {get; private set; }

     [field: SerializeField] public Attack[] Attacks {get; private set; }

    // [field: SerializeField] public PlayerStateMachine  PlayerMachine {get; private set; }

    // //   [field: SerializeField] public Health  PlayerHealth {get; private set; }

    // //   [field: SerializeField] public GameObject  PlayerGameCenterPivot {get; private set; }

    // [field: SerializeField] public GameObject  PlayerCharacterBlocker  {get; private set; }

    [field: SerializeField] public float HowLongBeforeAttack {get; private set; }


    [field: SerializeField] public float HowLongBeforePerformingAttack {get; private set; }


    [field: SerializeField] public float HowLongImpact {get; private set; }
    
    [field: SerializeField] public float Drag {get; private set; }
    
    [field:SerializeField] public float InRangePlayer {get; private set; }
    [field:SerializeField] public float Speed {get; private set; }
    
    [field:SerializeField] public float JumpSpeed {get; private set; }

    [field:SerializeField] public float FallMultiplayer {get; private set; }

    
    [field:SerializeField] public float LikeHoodContinueAttacking {get; private set;}
    
    [field:SerializeField] public bool IsAgressive {get; private set;}

    [field:SerializeField] public float HowLongBeforeJump {get; private set;}

   
    [field:SerializeField] public bool CanRoll {get; private set;}

    [field:SerializeField] public float LikeHoodRoll {get; private set;}
    
    [field:SerializeField] public bool CanGuard {get; private set;}

    [field:SerializeField] public float LikeHoodGuard {get; private set;}
    
    //private variables
    public Health PlayerHealth {get;private set;}

    public GameObject PlayerGameObjct {get;private set;}

    public GameObject PlayerGameCenterPivot {get;private set;}

    public PlayerStateMachine PlayerMachine {get;private set;}

    public GameObject PlayerCharacterBlocker {get;private set;}

    public bool FacingLeft {get; private set;}

    



    private void Awake(){
      
        //save color of sprite  
        
        PlayerGameObjct = GameObject.FindGameObjectWithTag("Player");
        PlayerMachine = PlayerGameObjct.GetComponent<PlayerStateMachine>();
        //get the health component of the player
        PlayerHealth = PlayerGameObjct.GetComponent<Health>();

        PlayerGameCenterPivot = GameObject.FindGameObjectWithTag("PivotCenterPlayer");
        PlayerCharacterBlocker = PlayerGameObjct.transform.Find("CharacterCollisonBlocker").gameObject;
    }


    // Change the bool regarind the last bool statate
    private void Flip(){
        FacingLeft = !FacingLeft;
        
        //attackMoveDirection.x *= -1;
        transform.Rotate(0,180,0);
    }


    public void FacePlayer(){
        //get the fireciton in onlu the x cordinate to know if it should rotate or not
        float dir = PlayerGameObjct.transform.position.x - transform.position.x;
        if(dir > 0 &&  FacingLeft){
            Flip();
        } else if(dir < 0 && ! FacingLeft){
            Flip();
        }
    }

    //IsGrounded
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }

    private void Update()
    {
        //Debug.Log(IsPlayerInRange());
        
        //Know in which state is player
        if(PlayerMachine == null){return;}
        //Debug.Log("This is the current state machine of the player: " +PlayerMachine.CurrentState);

    }

    public bool IsPlayerFacingEnemy(){
        
        float dotProduct = Vector3.Dot(PlayerGameObjct.transform.right, transform.right);
        //if the value is -1 it means that they are on the opposite direction
        // if it is 1 it means thare they are facing each other
        //with the dot product of the right component
        //facing with each other will be -1

        //Debug.Log(dotProduct);
        if(dotProduct<0){
            return true;
        }else{
            return false;
        }
    
    }


    public bool IsPlayerInRange(){
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position,InRangePlayer, PlayerLayer);
        //return Physics2D.OverlapCircle(transform.position,InRangePlayer,PlayerLayer);
        foreach(Collider2D col in hitColliders){
            
            //Debug.Log(col.gameObject.tag);
            if(col.CompareTag("Player")){
                return true;
            }
        }
        return false;
    }



    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,InRangePlayer);
       
        
    //     Gizmos.color = Color.green;
    //     //direction of palyer
    //    if(PlayerGameObjct ==null && PlayerGameCenterPivot == null){return;}
    //     //Vector3 dir = PlayerGameObjct.transform.position - transform.position;
    //     Gizmos.DrawLine(transform.position,PlayerGameCenterPivot.transform.position);
    }


   
    

}
