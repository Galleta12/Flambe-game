using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour
{
public Image healthBar;

// the max health that it can have each character on the game

// we want to manually set the health of each character
[field: SerializeField] public int maxhealth {get;private set;} = 100;
//the current health
private int currentHealth;
//set ivunerable for testing and for blocking
private bool isInvunerable;

private bool isDefend;


// events to check if the player is death or if is taking damage

public event Action<Vector2> OnTakeDamage;
//
public event Action OnDeath;
//bool se to check if is dead
public bool isDead => currentHealth == 0;

private int CounterHit;
public int counterHit{get{return CounterHit;}set{value = CounterHit;}}

private bool isNotBlockable;

public bool IsNotBlockable{get{return isNotBlockable;}set{isNotBlockable = value;}}



//private Vector2 guardKnockback;

public event Action<Vector2> OnGuardKnockback;

//get reference to the guard staming

private GuardStamina guard;


private void Start() {
   currentHealth = maxhealth;
   guard = GetComponent<GuardStamina>();
}


private void Update() {
   updateHealthBar();
}



public void setInvunerable(bool isInvunerable){
   this.isInvunerable = isInvunerable;
}
public void setIsDefend(bool isDefend){
   this.isDefend = isDefend;
}


public bool IsFront(int damage,Vector2 dir){
   
  
   Vector3 faceDir = transform.right;
   float hitDir = Vector2.Dot(dir,faceDir);
   //if the product is positive it means that the direction are pointing in the same
   //this means that the collision was from behind since we are comparing the direction of the hit with the right direction 
   
   //Debug.Log(hitDir);
   if(hitDir >=0){
      
      return true;
   }else{
      //if this is true we want to decrement the guard
    
      return false;
   }

}

public void DealDamage(int damage,Vector2 directionKnockback, Vector2 directionHit){
    // if the health is 0, we want dont want to do anything
    if(currentHealth == 0){return;}
    // if the character is invunerable we don't want to do anything
    if(isInvunerable){return;}

   guard.DealHit(damage);
   
     
   //if it is not behind we don't want to get damage
   if(!isNotBlockable && isDefend && !IsFront(damage,directionHit) && !guard.isBroke){
      //we want to get the knockback for the guard
      //we dont want the directionKnockback parameter divided by two. So we have half og the impact 
      //set the knockback
    
      
      OnGuardKnockback?.Invoke(directionKnockback);
      SoundManager.PlaySound("deflect");
      return;
   }
  
   // Debug.Log("This is :" + hitForGuard);
   
   //if what is above is not true we dont want to have a guard knockback
  
   //playe the impact sound 
   SoundManager.PlaySound("impact");
  
   
   
   //every time there is a hit either is defending or not we want to decrement the the guard stamina
   
    // currentHealth -= damage;
    // if(currentHealth < 0){
    //     currentHealth = 0;
    // }
    // this is a best way to do the same code that is abobe
    currentHealth = Mathf.Max(currentHealth - damage,0);

    
    //trigger the on take damage action and on death action for the enemy state machine
    //first we get the direction
   // Debug.Log(directionKnockback);
    OnTakeDamage?.Invoke(directionKnockback);
    //count how many times it was hit
    CounterHit++;
    if(currentHealth == 0){
      OnDeath?.Invoke();
    }
    
     
     //Debug.Log("This is the current health of the object: " + this.gameObject.name + "Health: " +currentHealth);
     //Debug.Log("Counter hit" + CounterHit);

}


public int GetHealth(){
   return currentHealth;
}

public bool IsSetInvunerable(){
   return isInvunerable;
}

public void setHealth(int increase){
   
   currentHealth +=increase;
   currentHealth = Mathf.Clamp(currentHealth,0,maxhealth);
}

private void updateHealthBar()
{
   healthBar.fillAmount = (currentHealth * 1f )/maxhealth;
}


   
}
