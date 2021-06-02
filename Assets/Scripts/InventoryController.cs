using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    ItemInventory itemInv = new ItemInventory();

    void Start() {
        
    }

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            itemInv.addStack(new ItemStack(Items.ITEM_TWIG, 1));
        } else if (Input.GetButtonDown("Fire2")) {
            itemInv.addStack(new ItemStack(Items.ITEM_BERRY, 5));
        } else if (Input.GetButtonDown("Fire3")) {
            ItemStack stack = itemInv.removeStack(new ItemStack(Items.ITEM_TWIG, 10));
            Debug.Log(stack.getItem().getName() + ": " + stack.getCount());
        } else if (Input.GetButtonDown("Jump")) {
            Debug.Log("Inventory Contents:");
            for(int i = 0; i < itemInv.getStackCount(); i++) {
                ItemStack s = itemInv.getStackAtIndex(i);
                Debug.Log(s.getItem().getName() + ": " + s.getCount());
            }
        }
    }
}
