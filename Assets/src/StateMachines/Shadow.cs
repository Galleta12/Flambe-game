using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    
    
    
    //static varaible
    //public static Shadow me;
    public GameObject Shadows;
    public List<GameObject> pool = new List<GameObject>();
    private float time;
    public float speed;

    public Color color;





    private void Awake() {
        //with this we can shadow from any other script
      //  me =this;
    }


    
    public GameObject GetShadow(){
        for(int i=0; i <pool.Count; i++){
        if(!pool[i].activeInHierarchy){
            pool[i].SetActive(true);
            pool[i].transform.position = transform.position;
            pool[i].GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            pool[i].GetComponent<SolidColor>().color = color;
            return pool[i];

        }
    }
         GameObject obj = Instantiate(Shadows, transform.position,transform.rotation) as GameObject;
        obj.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        obj.GetComponent<SolidColor>().color = color;
        pool.Add(obj);
        return obj;

    }


    public void ShadowSkill(){
        time += speed*Time.deltaTime;
        if(time >1){
            GetShadow();
            time= 0;
        }
    }
    

}
