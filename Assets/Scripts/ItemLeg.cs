using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ItemLeg : ItemArmor{

    public ItemLeg(string id, string name, string description, float dampening, float deflection)
        : base(id, name, description, dampening, deflection){

    }

    protected override void Equip(ItemStack stack, InventoryController inv, PlayerController player){
        if(player.getEquipedLeg() != null) 
            inv.addStack(new ItemStack(player.getEquipedLeg(), 1)); 
        player.equipLeg(this);
    }

}
