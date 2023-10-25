using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : Item{
    protected ItemType _Weapon;
    public enum ArmorType{
        Helmet,
        Gauntlets,
        ChestPiece,
        LegPiece,
        Boots,
        Amulet,
    }
    [Header("Effect Types")]
    [SerializeField] protected StrengthAndWeakness.ElementalType ElementalEffect;
    [SerializeField] protected StrengthAndWeakness.DamageType damageType;

    [SerializeField] protected int damage;
    [SerializeField] protected float reach;

    protected void Primary(){
        //do stuff here
    }
    protected void Secondary(){
        //do stuff here
    }
}
