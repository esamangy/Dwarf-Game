using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item{
    public enum ArmorType{
        Helmet,
        Gauntlets,
        ChestPiece,
        LegPiece,
        Boots,
        Amulet,
    }

    [Header("Resistances and weaknesses")]
    [SerializeField] protected StrengthAndWeakness.DamageType DamageResistance;
    [SerializeField] protected StrengthAndWeakness.DamageType DamageWeakness;
    //[SerializeField] protected StrengthAndWeakness.ElementalType ElementalResistance;
    //[SerializeField] protected StrengthAndWeakness.ElementalType Elemetalweakness;

    [Range(0,1)]
    [SerializeField] protected float armorPercent;
    [SerializeField] protected float armorWeight;


}
