using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour{
    //Armor Slots----------------------------
    public Armor helmet{ get; set; }
    public Armor chestpiece{ get; set; }
    public Armor legpiece{ get; set; }
    public Armor boots{ get; set; }
    public Armor gauntlents{ get; set; }
    public Armor amulet{ get; set; }
    //inventory-----------------------------
    [SerializeField] private float playerCarryWeight{ get;}
    private List<Dictionary<Item, int>> inventory;
    public bool hasBackpack{ get; set; }
    public Backpack backpack{ get; set; }
    public Weapon primary{ get; set; }
    public Weapon secondary{ get; set; }

    //returns total weight of inventory

    public void Start(){
        inventory = new List<Dictionary<Item, int>>();
    }
    public float getWeight(){
        return 1f;
    }

}
