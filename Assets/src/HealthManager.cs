using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health/100;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
