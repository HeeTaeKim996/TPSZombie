using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScene : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] RuntimeAnimatorController pistol,doublePistols;


    public void SetRunTimeController(bool singlePistol){
        if(singlePistol){
            animator.runtimeAnimatorController = pistol;
            animator.Play("Idle");
        }
        else{
            animator.runtimeAnimatorController = doublePistols;
            animator.Play("Idle");
        }
    }
    public void PlayAnimation(string animationName){
        animator.Play(animationName);
    }
}
