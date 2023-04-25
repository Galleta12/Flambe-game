using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GuardStamina : MonoBehaviour
{
    [field: SerializeField] public int MaxStamina {get;private set;} = 100;

    [field: SerializeField] public float HowLongBroke {get;private set;}

    [field: SerializeField] public float HowManySecondsIncrement {get;private set;}

    [field: SerializeField] public int HowManyStepsCanIncrement {get;private set;}


    private int currentStamina;
    
    public bool isBroke => currentStamina== 0;

    private float remainingTime;

    public Image staminaBar;


 



    private void Start() {
        currentStamina = MaxStamina;
        remainingTime = HowLongBroke;
        StartCoroutine("Increment",HowManySecondsIncrement);

    }

    private void UpdateStaminaBar(){
        staminaBar.fillAmount = (1f * currentStamina)/MaxStamina;
    }


    private void Update() {
        //check if the current Stamine is 0
        UpdateStaminaBar();   
        if(currentStamina ==0){
            //decrement the fixed time of how long it can be broke
            remainingTime -= Time.deltaTime;
            if(remainingTime <=0){
                
                remainingTime = HowLongBroke;
                //set back the values since the time is over
                currentStamina = MaxStamina;
                //start courotine again
                StartCoroutine("Increment",HowManySecondsIncrement);
            }
        }    
     
    }

    public void DealHit(int damage){
        
        
        if(currentStamina ==0){return;}
        //if(isBroke){return;}
        currentStamina = Mathf.Max(currentStamina - damage,0);

        if(currentStamina ==0){
          
            //stop courotine
            StopCoroutine("Increment");
        }
    }

    public void SetStamina(){
        currentStamina = MaxStamina;
    }

    public int GetGuardStamina(){
        return currentStamina;
    }


    IEnumerator Increment(float delay){
        
        
        while(true){
            yield return new WaitForSeconds(delay);
            //Debug.Log("This courotine is being called");
            //call the funciton to start finding the path
            currentStamina += HowManyStepsCanIncrement;
            //clamp value, min, max parameters
            currentStamina = Mathf.Clamp(currentStamina,0,MaxStamina);

        }
    }

 


}
