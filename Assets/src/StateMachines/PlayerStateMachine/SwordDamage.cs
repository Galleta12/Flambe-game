using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
//[field: SerializeField] public Rigidbody RB {get; private set;}
//avoid game with itself
[field: SerializeField] public CapsuleCollider2D   mycharacterController {get; private set;}
// this is to keep track of the object that collide with the sword
private List<Collider2D> alreadyCollideWith = new List<Collider2D>();

 private int damage;
 //this is for the knockback of the sword
 private float knockback;


   
   
   private void OnEnable() {
    // we want to clear the list of the objects that we collide
    // as soon as this object is enable
    alreadyCollideWith.Clear();
    //Debug.Log("hello");
   
   }


   private void OnDisable()
   {
     //Debug.Log("Bye");
   }

   
   private void OnTriggerEnter2D(Collider2D other) {
     //we dont want to do anything is the sword hits our collider
    if(other == mycharacterController){return;}
    //if the object already exists on the list
    //it means that we already hit it, therefore we dont want to do anything
    if(alreadyCollideWith.Contains(other)){return;}
        alreadyCollideWith.Add(other);
    // we want to get the health component of the object that we collide with
    if(other.TryGetComponent<Health>(out Health health) && other.CompareTag("Enemy")){
       
        //Debug.Log("Collision with this object" + other.gameObject.name);
       
       // we want to pass the damage and the knockback, therefore we can hanlde the impact state
        // we pass the direction for the knockback        
        Vector2 direction = (other.transform.position - mycharacterController.transform.position).normalized;
        //get the direction from the player position and the collision
        health.DealDamage(damage,direction * knockback,direction);
        
    }
    
   }

    // with this we set the attack damage
   public void SetAttack(int damage, float knockback){
      this.damage = damage;
      this.knockback = knockback;
   }
}
