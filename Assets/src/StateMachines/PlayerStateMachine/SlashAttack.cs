using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    public float speed = 5f, radius = 0.2f;
    public int hits = 1;


    public float distanceHitPlayer = 5f;
    public LayerMask hitMask = int.MaxValue;

    public ParticleSystem SlashEffect;
    private int damage;
    //this is for the knockback of the sword
    private float knockback;
    private Vector2 dirToMouse;
    private GameObject PlayerGameObject;
    
    void Start()
    {
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
        SlashEffect.Play();    
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
            castHit = Physics2D.CircleCast(transform.position, radius, dirToMouse, distance, hitMask);
        }
        else
        {
            castHit = Physics2D.Raycast(transform.position, dirToMouse, distance, hitMask);
        }

        if(castHit)
        {
            
             Debug.Log("This is what it hit with the slash" + castHit.collider.name);
             hits--;
            //Debug.Log("Cast hit" + castHit.collider.name);
            //DealDamage
            if(castHit.collider.TryGetComponent<Health>(out Health health)&&
            castHit.collider.CompareTag("Enemy")){
                
                //distance of the player with the enemy collision
                Vector2 direction = (castHit.collider.transform.position - PlayerGameObject.transform.position).normalized;
               float distancePlayerEnemy = Vector2.Distance(PlayerGameObject.transform.position,castHit.collider.transform.position);
                //Debug.Log("THis is the distance" + distancePlayerEnemy);
                //Debug.Log("This is the knockback for the slash" + knockback);
                if(distancePlayerEnemy < distanceHitPlayer){
                    //Debug.Log("Distance collison");
                    health.DealDamage(damage,direction * knockback,direction);
                }else{
                    health.DealDamage(damage,Vector2.zero,direction);
                }
            }
           
        }
        transform.position += distance * (Vector3)dirToMouse;
    }

    public void SetAttack(int damage, float knockback, Vector2 dirToMouse){
      this.damage = damage;
      this.knockback = knockback;
      this.dirToMouse= dirToMouse;
   }
    void OnDrawGizmos()
    {
        if (radius > 0f)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        Gizmos.color = Color.red;
        var dist = transform.position + speed * 0.1f * (Vector3)dirToMouse;
        Gizmos.DrawLine(transform.position, dist);
        Gizmos.DrawLine(dist, dist + new Vector3(-0.1f,-0.1f,0));
        Gizmos.DrawLine(dist + new Vector3(-0.1f, -0.1f, 0), dist + new Vector3(0.1f,-0.1f,0));
        Gizmos.DrawLine(dist + new Vector3(0.1f, -0.1f, 0), dist);

    }
}
