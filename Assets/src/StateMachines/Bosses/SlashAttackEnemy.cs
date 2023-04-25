using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttackEnemy : MonoBehaviour
{
    public float speed = 5f, radius = 0.2f;
    public int hits = 1;


    public float distanceHitPlayer = 5f;
    public LayerMask hitMask = int.MaxValue;

    //public ParticleSystem SlashEffect;
     private int damage;
    //this is for the knockback of the sword
    private float knockback;
    private bool isNotBlockable;

    private Vector2 directionMove;
    
    private GameObject EnemyGameObject;
    private GameObject PlayerGameObject;

    
    void Start()
    {
        EnemyGameObject = GameObject.FindGameObjectWithTag("Enemy");
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
        this.directionMove = (PlayerGameObject.transform.position-this.transform.position).normalized;

        //SlashEffect.Play();    
    }
    
    
    
    void FixedUpdate()
    {
        if(hits <= 0) 
        {
            Destroy(gameObject);
        }
        var distance = speed * Time.fixedDeltaTime;
        RaycastHit2D castHit;
        if(radius >= 0f)
        {
            castHit = Physics2D.CircleCast(transform.position, radius, directionMove, distance, hitMask);
        }
        else
        {
            castHit = Physics2D.Raycast(transform.position, directionMove, distance, hitMask);
        }

        if(castHit)
        {
              
             //Debug.Log("This is what it hit with the slash " + castHit.collider.name);
           
             //I dont want to detect collision with myself
             if(!castHit.collider.CompareTag("Enemy")){
                   hits--;                
                    if(castHit.collider.TryGetComponent<Health>(out Health health)&&
                    castHit.collider.CompareTag("Player")){
           
                       
                        Vector2 direction = (castHit.collider.transform.position - EnemyGameObject.transform.position).normalized;
                        float distancePlayerEnemy = Vector2.Distance(EnemyGameObject.transform.position,castHit.collider.transform.position);
                        health.IsNotBlockable = isNotBlockable;
                        if(distancePlayerEnemy < distanceHitPlayer){
                        
                            health.DealDamage(damage,direction * knockback,direction);
                        }else{
                            health.DealDamage(damage,Vector2.zero,Vector2.zero);
                        }
                    }
             }
           
        }
        ///ransform.position += distance * directionMove;
         //Debug.DrawRay(transform.position,directionMove, Color.black, speed);
         transform.position += new Vector3(directionMove.x, directionMove.y,0f) * speed * Time.deltaTime;
    }

    public void SetAttack(int damage, float knockback, bool isNotBlockable){
      this.damage = damage;
      this.knockback = knockback;
      //this.directionMove = directionToMove;
      this.isNotBlockable = isNotBlockable;
   }
    void OnDrawGizmos()
    {
        if (radius > 0f)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        Gizmos.color = Color.red;
        var dist = transform.position + speed * 0.1f * (Vector3)directionMove;
        Gizmos.DrawLine(transform.position, dist);
        Gizmos.DrawLine(dist, dist + new Vector3(-0.1f,-0.1f,0));
        Gizmos.DrawLine(dist + new Vector3(-0.1f, -0.1f, 0), dist + new Vector3(0.1f,-0.1f,0));
        Gizmos.DrawLine(dist + new Vector3(0.1f, -0.1f, 0), dist);

    }
}
