using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable {

    public ItemStack item;
    Sprite tex;

    // Start is called before the first frame update
    void Start() {
        item = new ItemStack(Items.ITEM_TWIG, 10);
        tex = Resources.Load<Sprite>("Sprites/"+item.getItem().getName().Replace(":", "/"));
        Debug.Log("Sprites/"+item.getItem().getName().Replace(":", "/"));
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
