using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ItemBoot : ItemArmor{

    public ItemBoot(string id, string name, string description, float dampening, float deflection)
        : base(id, name, description, dampening, deflection){

    }

    protected override void Equip(ItemStack stack, InventoryController inv, PlayerController player){
        if(player.getEquipedBoot() != null) 
            inv.addStack(new ItemStack(player.getEquipedBoot(), 1)); 
        player.equipBoot(this);
    }

}
