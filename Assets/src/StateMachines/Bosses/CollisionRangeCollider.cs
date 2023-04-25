using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionRangeCollider : MonoBehaviour
{
    
    public BoxCollider2D ColliderRange{get; private set;}
    
    private GameObject PlayerGameObject;    
    //action to check if the enemy is moving toward the enemy
    public event Action OnSlashMovingToward;
    
    
    private void Start() {
        ColliderRange = GetComponent<BoxCollider2D>();
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        
        //Debug.Log(other.tag);
        
        if(other.CompareTag("SlashPlayer")){
            //Debug.Log("THe is a hit");
            OnSlashMovingToward?.Invoke();
        }
        
        
        // if(other.CompareTag("SlashPlayer")){
            
        //     Transform currentPositionEnemy = transform;
        //     Transform otherPosition = other.transform;
        //     //Check if the object are facing the same direction
        //     Vector2 difference = otherPosition.transform.position - currentPositionEnemy.position;
        //     Vector2 directiion1 = currentPositionEnemy.transform.right;
        //     float dotProduct = Vector2.Dot(difference.normalized,directiion1.normalized);
        //     //if the dot product is positive it means they are facing on the same direction
        //     if(dotProduct > 0){
        //         OnPlayerMovingToward?.Invoke();
        //     }
            
        //     //Debug.Log(dotProduct);
        // }

    }

    

}
