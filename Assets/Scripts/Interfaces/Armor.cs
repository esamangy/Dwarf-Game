using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Armor : Item{
    protected ItemType _Armor;
    public enum ArmorType{
        Helmet,
        Gauntlets,
        ChestPiece,
        LegPiece,
        Boots,
        Amulet,
    }

    [Header("Resistances and weaknesses")]
    [SerializeField] protected StrengthAndWeakness.DamageType DResistance;
    [SerializeField] protected StrengthAndWeakness.DamageType DWeakness;
    [SerializeField] protected StrengthAndWeakness.ElementalType EResistance;
    [SerializeField] protected StrengthAndWeakness.ElementalType Wweakness;

    [SerializeField] protected float armorPercent;
    [SerializeField] protected float armorWeight;


}
