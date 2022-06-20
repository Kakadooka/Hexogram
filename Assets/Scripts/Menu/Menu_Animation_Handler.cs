using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Animation_Handler : MonoBehaviour
{
    Animator animator;
    Menu_Handler menuHandler;
    bool enableBoxCollidersTrigger = false;
    Sound_Handler soundHandler;

    void Start(){
        animator = gameObject.GetComponent<Animator>();
        menuHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Menu_Handler>();
        soundHandler = GameObject.Find("!SCRIPT HOLDER!").GetComponent<Sound_Handler>();
    }

    public void makeTutorialGoUp(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("stayDown")){
            animator.SetTrigger("goUp");
            soundHandler.playUncrumblePaper();

            menuHandler.layerMask = 0;
        }
    }
    public void makeTutorialGoDown(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("stayUp")){
            animator.SetTrigger("goDown");
            soundHandler.playCrumblePaper();

            menuHandler.layerMask = 1;
        }
    }

    void OnMouseDown(){
        makeTutorialGoDown();
    }

}
