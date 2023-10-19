using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour{
    //Movement------------------------------
    [Header("Stats")]
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int mana;
    [SerializeField] protected int maxMana;
    [SerializeField] protected int stamina;
    [SerializeField] protected int maxStamina;
    //--------------------------------------

    public abstract void Hurt(int damange);
    public abstract void Heal(int amount);
    public abstract void SpendMana(int amount);
    public abstract void RestoreMana(int amount);
    public abstract void SpendStamina(int amount);
    public abstract void RestoreStamina(int amount);
    public override string ToString(){
        string healthstr = health.ToString() + "/" + maxHealth.ToString() + " hp\n";
        string staminastr = stamina.ToString() + "/" + maxStamina.ToString() + " sp\n";
        string manastr = mana.ToString() + "/" + maxMana.ToString() + " mp\n";
        return healthstr + staminastr + manastr;
    }
    public virtual string ToString(string str){
        string healthstr = health.ToString() + "/" + maxHealth.ToString() + " hp\n";
        string staminastr = stamina.ToString() + "/" + maxStamina.ToString() + " sp\n";
        string manastr = mana.ToString() + "/" + maxMana.ToString() + " mp\n";
        return str + healthstr + staminastr + manastr;
    }
}
