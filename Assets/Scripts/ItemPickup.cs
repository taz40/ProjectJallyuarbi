using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable {

    public ItemStack item;
    Sprite tex;

    // Start is called before the first frame update
    void Start() {
        if(item == null)
            setItem(new ItemStack(Items.ITEM_TWIG, 10));
    }

    public void setItem(ItemStack stack){
        item = stack;
        tex = item.getItem().getSprite();
        GetComponentInChildren<SpriteRenderer>().sprite = tex;
    }

    // Update is called once per frame
    void Update() {

    }

    public override void Interact(GameObject interactor) {
        InventoryController._instance.addStack(item);
        Destroy(this.gameObject);
    }
}
