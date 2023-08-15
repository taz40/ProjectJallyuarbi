using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory {
    List<ItemStack> inv = new List<ItemStack>();

    public void addStack(ItemStack stack) {
        bool merge = false;
        foreach (ItemStack s in inv) {
            if (s.getItem() == stack.getItem()) {
                if(s.getCount() >= s.getItem().getMaxStackSize())
                    continue;
                if(s.getCount() + stack.getCount() > s.getItem().getMaxStackSize()){
                    s.incCount(s.getItem().getMaxStackSize() - s.getCount());
                    stack.decCount(s.getItem().getMaxStackSize() - s.getCount());
                    continue;
                }
                s.incCount(stack.getCount());
                return;
            }
        }
        inv.Add(stack);
    }

    public ItemStack removeStack(ItemStack stack) {
        foreach (ItemStack s in inv) {
            if (s.getItem() == stack.getItem()) {
                if(s.getCount() > stack.getCount()) {
                    s.decCount(stack.getCount());
                    return stack;
                } else {
                    inv.Remove(s);
                    return s;
                }
            }
        }
        return new ItemStack(stack.getItem(), 0);
    }

    public int getStackCount() {
        return inv.Count;
    }

    public ItemStack getStackAtIndex(int index) {
        return inv[index].copy();
    }

}
