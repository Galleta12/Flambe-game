using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AirAttackData
{
[field: SerializeField] public string AnimationName {get; private set;}
  
  [field: SerializeField] public float TransitionDuration {get; private set;}

  [field: SerializeField] public float AirAttackTime {get; private set;} 

  [field: SerializeField] public float ForceTime {get; private set;} 

  [field: SerializeField] public float Force {get; private set;} 

  [field: SerializeField] public float ForceSlashThrow {get; private set;} 
 
  [field: SerializeField] public int Damage {get; private set;}

  [field: SerializeField] public float Knockback {get; private set;}

  [field: SerializeField] public bool isNotBlockable {get; private set;}  

}
