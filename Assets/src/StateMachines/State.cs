using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Enter();

    public abstract void Tick(float deltaTime);

    public abstract void FixedTick(float fixeddeltaTime);


    public abstract void Exit();

    protected float GetNormalizedTime(Animator animator){
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if(animator.IsInTransition(0) && nextInfo.IsTag("Attack")){
            return nextInfo.normalizedTime;
        }else if(!animator.IsInTransition(0) && currentInfo.IsTag("Attack")){
            return currentInfo.normalizedTime;
        }else{
            return 0f;
        }
    }
    protected float GetNormalizedTimeForAll(Animator animator, string tag){
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if(animator.IsInTransition(0) && nextInfo.IsTag(tag)){
            return nextInfo.normalizedTime;
        }else if(!animator.IsInTransition(0) && currentInfo.IsTag(tag)){
            return currentInfo.normalizedTime;
        }else{
            return 0f;
        }
    }

}

