using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack {

    Item item;
    int count;

    public ItemStack(Item item, int count = 1) { 
        if(count > item.getMaxStackSize()) {
            count = item.getMaxStackSize();
        }
        this.item = item;
        this.count = count;
    }

    public Item getItem() {
        return item;
    }

    public int getCount() {
        return count;
    }

    public void incCount(int amount = 1) {
        count += amount;
        if (count > item.getMaxStackSize()) {
            count = item.getMaxStackSize();
        }
    }

    public void decCount(int amount = 1) {
        count -= amount;
        if (count <= 0) {
            count = 0;
        }
    }

    public ItemStack copy() {
        return new ItemStack(item, count);
    }

}
