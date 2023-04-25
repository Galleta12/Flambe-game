using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidColor : MonoBehaviour
{
    
    private SpriteRenderer myrenderer;
    private Shader myMaterial;
    public  Color color;
    
    
    private void Start()
    {
       
        myrenderer = GetComponent<SpriteRenderer>();
        myMaterial = Shader.Find("GUI/Text Shader");        
    }

    // Update is called once per frame
    private void Update()
    {
        ColorSprite();
    }


    private void ColorSprite(){
        myrenderer.material.shader = myMaterial;
        myrenderer.color = color;
    }

    public void Finish(){
        gameObject.SetActive(false);
    }
}
