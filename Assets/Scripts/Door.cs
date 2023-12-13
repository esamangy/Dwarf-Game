using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable{
    private Animator anim;
    private bool isOpen = false;
    [SerializeField] private string openStateText;
    private string closeStateText;
    public override void Awake(){
        base.Awake();
        anim = GetComponent<Animator>();
        closeStateText = hoverText;
    }
    public override void interact(){
        if(isOpen){
            anim.Play("DoorClose", 0, 0f);
            isOpen = false;
            hoverText = closeStateText;
        } else {
            anim.Play("DoorOpen", 0, 0f);
            isOpen = true;
            hoverText = openStateText;
        }
    }

    public override void Update(){
        base.Update();
    }

    public override void Highlight(){
        base.Highlight();
    }
}
