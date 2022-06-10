using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ItemArmor : Item{

    float dampening;
    float deflection;

    public ItemArmor(string id, string name, string description, float dampening, float deflection)
        : base(id, name, description){
        this.dampening = dampening;
        this.deflection = deflection;

    }

    public override int getMaxStackSize() {
        return 1;
    }

    protected abstract void Equip(ItemStack stack, InventoryController inv, PlayerController player);

    public override Dictionary<string, Action<ItemStack>> getItemActions(Dictionary<string, Action<ItemStack>> actions){
        actions.Add("Equip", (stack) => {Equip(stack, InventoryController._instance, InventoryController._instance.getPlayer()); InventoryController._instance.destroyStack(stack); InventoryController._instance.deselect(); });
        return base.getItemActions(actions);
    }

}
