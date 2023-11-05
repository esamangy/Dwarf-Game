using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable{
    public override void Awake(){
        base.Awake();
        hoverText = "Pick up " + name;
    }
    public override void interact(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInventory>().addItemToInventory(gameObject.GetComponent<Item>());

    }

    public override void Update(){
        base.Update();
    }

    public override void Highlight(){
        base.Highlight();
    }
}
