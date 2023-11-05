using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour{
    private class InventroyType{
        public Item.ItemType m_type;
        public List<InventroyItem> itemsInCategory;

        public InventroyType(Item.ItemType t){
            m_type = t;
            itemsInCategory = new List<InventroyItem>();
        }
    }
    public class InventroyItem{
        public string name;
        public int numItems;
        public float itemWeight;
        public InventroyItem(){
            name = "";
            numItems = 0;
            itemWeight = 0;
        }
        public InventroyItem(string n, int num, float w){
            name = n;
            numItems = num;
            itemWeight = w;
        }
    }
    //Armor Slots----------------------------
    public Armor helmet{ get; set; }
    public Armor chestpiece{ get; set; }
    public Armor legpiece{ get; set; }
    public Armor boots{ get; set; }
    public Armor gauntlents{ get; set; }
    public Armor amulet{ get; set; }
    //inventory-----------------------------
    [SerializeField] private float playerCarryWeight{ get;}
    private List<InventroyType> inventory;
    public bool hasBackpack{ get; set; }
    public Backpack backpack{ get; set; }
    public Weapon primary{ get; set; }
    public Weapon secondary{ get; set; }
    //returns total weight of inventory

    public void Awake(){
        inventory = new List<InventroyType>();
    }
    public void Start(){
        setupInventory();
    }
    public float getWeight(){
        return 1f;
    }

    public void addItemToInventory(Item itemToAdd){
        foreach(InventroyType category in inventory){
            if(category.m_type == itemToAdd.getType()){
                foreach(InventroyItem item in category.itemsInCategory){
                    if(item.name == itemToAdd.name){
                        item.numItems += itemToAdd.numItems;
                        Destroy(itemToAdd.gameObject);
                        break;
                    }
                }
                category.itemsInCategory.Add(new InventroyItem(itemToAdd.name, itemToAdd.numItems, itemToAdd.weight));
                Destroy(itemToAdd.gameObject);
                break;
            }
        }
    }

    //returns true if successfully removed, false otherwise
    //only ueful for completely deleting item from inventory
    public bool removeItemFromInventory(InventroyItem itemToRemove){
        foreach(InventroyType category in inventory){
            foreach(InventroyItem item in category.itemsInCategory){
                if(item == itemToRemove){
                    category.itemsInCategory.Remove(item);
                    return true;
                }
            }
        }
        return false;
    }

    private void setupInventory(){
        foreach(Item.ItemType t in System.Enum.GetValues(typeof(Item.ItemType))){
            inventory.Add(new InventroyType(t));
        }
    }
}
