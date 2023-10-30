using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StrengthAndWeakness : ScriptableObject{
    public enum ElementalType{
        none,
        Fire,
        Electric,
        Poison,
        Earth,
        Demonic,
        Holy
    }

    public enum DamageType{
        Slashing,
        Piercing,
        Smashing,
        Magical,
        Psionic
    }

    
}
