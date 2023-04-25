using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBounceHead : MonoBehaviour
{
   
    
    [field:SerializeField] public bool isPlayer {get;private set;}
    
    
    private void OnTriggerEnter2D(Collider2D other) {
        
        
        //Debug.Log(other.gameObject.name);
        Vector3 contact = other.transform.position;
        //avoid collision with itself
        if(isPlayer){
            if(!other.CompareTag("Enemy")){return;}
        }else{
            if(!other.CompareTag("Player")){return;}
        }
        
        //for now just jump. Maybe Create another bounce state for the player
        //SwitchState(new PlayerJumpState(this));

    }
}
