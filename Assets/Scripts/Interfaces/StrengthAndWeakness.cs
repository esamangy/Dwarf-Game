using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[ExecuteInEditMode]
public class StrengthAndWeakness : ScriptableObject{

    public enum DamageType{
        Slashing,
        Piercing,
        Smashing,
        Magical,
        Psionic
    }

    //always do attach defence
    public float[,] DamageLookUp = new float[,]{
                        //Slashing  Piercing    Smashing    Magical     Psionic
        /*Slashing*/    {.5f,       1.5f,       1f,         1f,         1f},
        /*Piercing*/    {1f,        .5f,        1.5f,       1f,         1f},
        /*Smashing*/    {1.5f,      1f,         .5f,        1f,         1f},
        /*Magical*/     {1f,        1f,         1f,         .5f,        1.5f},
        /*Psionic*/     {1f,        1f,         1f,         1.5f,       .5f}
    };
    
    //always do attach defence
    public float[,] ElementLookUp = new float[,]{
                    //Dark      Light       Earth       Fire        Poison      Electric
        /*Dark*/    {1f,        1.5f,       1f,         .5f,        1f,         .8f},
        /*Light*/   {1.5f,      1f,         1f,         .8f,        1f,         1f},
        /*Earth*/   {1f,        1f,         .8f,        1f,         .8f,        1.5f},
        /*Fire*/    {1.5f,      .8f,        1f,         .8f,        1f,         1f},
        /*Poison*/  {1f,        .8f,        1f,         1f,         .5f,        1f},
        /*Electric*/{1.5f,      .8f,        .5f,        1f,         1f,         .5f}
    };
    
}
