using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject{
    protected enum ItemType{
        _Armor,
        _Food,
        _Resource,
        _Weapon,
        _Misc,
        _Backpack,
        _Debug
    }

    [SerializeField] private float weight{get;}
}
