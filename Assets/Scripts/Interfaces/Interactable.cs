using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Interactable : MonoBehaviour{
    private Outline outline;
    private bool highlight = false;
    public abstract void interact();

    public virtual void Awake(){
        outline = GetComponent<Outline>();
        if(!outline){
            this.AddComponent<Outline>();
        }
        outline.enabled = false;
    }

    public virtual void Update(){
        outline.enabled = highlight;
        highlight = false;
    }

    public virtual void Highlight(){
        highlight = true;
    }
}
