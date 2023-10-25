using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour{
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
