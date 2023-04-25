using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAttackSpecial : MonoBehaviour
{
    public float maxspeed = 5f;

    private float speed;

    public float remaingTime;
    
    public int damage;

    private Rigidbody2D rb;

    private Animator Animator;

    
    //private Vector2 directionMove;
    
    private GameObject EnemyGameObject;
    private GameObject PlayerGameObject;
    private bool throwNow;
    private bool isThrowed;

    private readonly int SPHash = Animator.StringToHash("water_special");

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        EnemyGameObject = GameObject.FindGameObjectWithTag("Enemy");
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
        speed = Random.Range(2,maxspeed +1);
        
    }



    
    private void FixedUpdate() {
        
        if(throwNow){
            if(!isThrowed){
                Vector2 directionMove = (PlayerGameObject.transform.position-this.transform.position).normalized;
                
                rb.AddForce(directionMove * speed,ForceMode2D.Force);
            }
        }
    
    }
    
    private void Update()
    {
        remaingTime -= Time.deltaTime;

        if(remaingTime <=0){
            Animator.Play(SPHash);
            throwNow = true;
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.TryGetComponent<Health>(out Health health)
            &&other.CompareTag("Player")){ 
                health.DealDamage(damage,Vector2.zero,Vector2.zero);
                Destroy(gameObject, 1f);
                return;
        }
        if(other.CompareTag("Ground")){
            Destroy(gameObject, 1f);
        }
       

    }

}
