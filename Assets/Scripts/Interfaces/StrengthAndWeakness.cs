using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthAndWeakness : MonoBehaviour{
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
