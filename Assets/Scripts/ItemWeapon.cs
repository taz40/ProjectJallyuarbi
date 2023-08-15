using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemWeapon : Item{

    float damage;
    float damageVariance;
    float minDamage;

    public ItemWeapon(string id, string name, string description, float damage, float damageVariance, float minDamage = 1)
        : base(id, name, description){
        this.damage = damage;
        this.damageVariance = damageVariance;
        this.minDamage = minDamage;

    }

    public float getDamage() {
        return UnityEngine.Random.Range(Mathf.Max(minDamage, damage - damageVariance), damage + damageVariance);
    }

    public override int getMaxStackSize() {
        return 1;
    }

    public override Dictionary<string, Action<ItemStack>> getItemActions(Dictionary<string, Action<ItemStack>> actions){
        actions.Add("Equip", (stack) => {if(InventoryController._instance.getPlayer().getEquipedWeapon() != null) InventoryController._instance.addStack(new ItemStack(InventoryController._instance.getPlayer().getEquipedWeapon(), 1)); InventoryController._instance.getPlayer().equipWeapon(this); InventoryController._instance.destroyStack(stack); InventoryController._instance.deselect(); });
        return base.getItemActions(actions);
    }

}
