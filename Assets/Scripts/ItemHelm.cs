using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ItemHelm : ItemArmor{

    public ItemHelm(string id, string name, string description, float dampening, float deflection)
        : base(id, name, description, dampening, deflection){

    }

    protected override void Equip(ItemStack stack, InventoryController inv, PlayerController player){
        if(player.getEquipedHelm() != null) 
            inv.addStack(new ItemStack(player.getEquipedHelm(), 1)); 
        player.equipHelm(this);
    }

}
