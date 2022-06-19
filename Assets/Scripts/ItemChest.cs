using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ItemChest : ItemArmor{

    public ItemChest(string id, string name, string description, float dampening, float deflection)
        : base(id, name, description, dampening, deflection){

    }

    protected override void Equip(ItemStack stack, InventoryController inv, PlayerController player){
        if(player.getEquipedChest() != null) 
            inv.addStack(new ItemStack(player.getEquipedChest(), 1)); 
        player.equipChest(this);
    }

}
