using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : Item{

    float damage;
    float damageVariance;
    float minDamage;

    public ItemWeapon(string name, float damage, float damageVariance, float minDamage = 1)
        : base(name){
        this.damage = damage;
        this.damageVariance = damageVariance;
        this.minDamage = minDamage;

    }

    public float getDamage() {
        return Random.Range(Mathf.Max(minDamage, damage - damageVariance), damage + damageVariance);
    }

    public override int getMaxStackSize() {
        return 1;
    }

}
