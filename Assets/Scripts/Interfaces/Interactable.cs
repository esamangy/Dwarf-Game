using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour{
    private Outline outline;
    [SerializeField] protected string hoverText;
    private bool highlight = false;
    public abstract void interact();
    private GameObject hoverTextObject;

    public virtual void Awake(){
        outline = GetComponent<Outline>();
        if(!outline){
            gameObject.AddComponent<Outline>();
        }
        outline.enabled = false;
        hoverTextObject = GameObject.Find("Interact Hover");
    }

    public virtual void Update(){
        outline.enabled = highlight;
        highlight = false;
    }

    public virtual void Highlight(){
        highlight = true;
        hoverTextObject.GetComponent<HoverTextController>().UpdateText(hoverText);
    }
}
