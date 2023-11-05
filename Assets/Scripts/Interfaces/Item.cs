using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour{
    public enum ItemType{
        _Armor,
        _Weapon,
        _Equipment,
        _Food,
        _Resource,
        _Misc,
    }
    protected ItemType m_type;
    public string _name;
    public float weight;
    public int numItems;
    public ItemType getType(){
        return m_type;
    }
}
