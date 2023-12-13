using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour{
    private Outline outline;
    [SerializeField] protected string hoverText;
    private bool highlight = false;
    public abstract void interact();
    private PlayerController player;

    public virtual void Awake(){
        outline = GetComponent<Outline>();
        if(!outline){
            gameObject.AddComponent<Outline>();
        }
        outline.enabled = false;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public virtual void Update(){
        outline.enabled = highlight;
        highlight = false;
    }

    public virtual void Highlight(){
        highlight = true;
        player.getPlayerHUD().hoverText = this.hoverText;
        player.statusChanged.Invoke();
        StartCoroutine(returnToBlank());
    }

    private IEnumerator returnToBlank(){
        while(highlight){
            yield return null;
        }
        player.getPlayerHUD().hoverText = "";
        player.statusChanged.Invoke();
    }
}
