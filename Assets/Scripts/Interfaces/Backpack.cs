using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Backpack : Item{
    protected ItemType _Backpack;
    [Range(0,1)]
    [SerializeField] protected float weightReduction;

}
