using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item{
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

    protected abstract void Primary();
    protected abstract void Secondary();
}
