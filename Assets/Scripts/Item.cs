using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    string id;
    string name;
    string description;
    Sprite sprite;

    public Item(string id, string name, string description) {
        this.name = name;
        this.id = id;
        this.description = description;
        sprite = Resources.Load<Sprite>("Sprites/"+id.Replace(":", "/"));
    }

    public string getName() {
        return name;
    }

    public string getID() {
        return id;
    }

    public Sprite getSprite() {
        return sprite;
    }

    public string getDescription() {
        return description;
    }

    public virtual int getMaxStackSize() {
        return int.MaxValue;
    }

    public Dictionary<string, Action<ItemStack>> getItemActions(Dictionary<string, Action<ItemStack>> actions){
        actions.Add("Drop", (stack) => {InventoryController._instance.dropStack(stack); InventoryController._instance.deselect(); });
        return actions;
    }

}
