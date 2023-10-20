using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour{
    //Movement------------------------------
    [Header("Stats")]
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float mana;
    [SerializeField] protected float maxMana;
    [SerializeField] protected int stamina;
    [SerializeField] protected int maxStamina;
    //--------------------------------------

    public abstract void Hurt(int damange);
    public abstract void Heal(int amount);
    public abstract void SpendMana(float amount);
    public abstract void RestoreMana(float amount);
    public abstract void SpendStamina(int amount);
    public abstract void RestoreStamina(int amount);
    public override string ToString(){
        string end = string.Format("{1}/{2} hp\n{3}/{4} sp\n{5:0.##}/{6} mp\n", health, maxHealth, stamina, maxStamina, mana, maxMana);
        return end;
    }
    public virtual string ToString(string str){
        string end = string.Format("{0}{1}/{2} hp\n{3}/{4} sp\n{5:0.##}/{6} mp\n", str, health, maxHealth, stamina, maxStamina, mana, maxMana);
        return end;
    }
}
