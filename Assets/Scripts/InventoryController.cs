using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

    ItemInventory itemInv = new ItemInventory();
    public static InventoryController _instance;
    public GameObject InventoryCanvas;
    public GameObject InventoryElement;
    public GameObject ItemSpawnLocation;
    public GameObject ItemPickup;

    void Start() {
        if(_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            GameObject go = Instantiate(ItemPickup, ItemSpawnLocation.transform.position, ItemSpawnLocation.transform.rotation);
            Item item = null;
            switch(Random.Range(0,4)){
                case 0:
                    item = Items.ITEM_TWIG;
                    break;
                case 1:
                    item = Items.ITEM_BERRY;
                    break;
                case 2:
                    item = Items.WEAPON_SWORD;
                    break;
                case 3:
                    item = Items.KEY_ITEM_GATE_KEY;
                    break;
                default:
                    break;
            }
            if(item == Items.KEY_ITEM_GATE_KEY || item == Items.WEAPON_SWORD){
                go.GetComponent<ItemPickup>().setItem(new ItemStack(item, 1));
            }else{
                go.GetComponent<ItemPickup>().setItem(new ItemStack(item, Random.Range(1, 31)));
            }
        }

        if(Input.GetButtonDown("Inventory")){
            if(InventoryCanvas.activeSelf){
                InventoryCanvas.SetActive(false);
            }else{
                InventoryCanvas.SetActive(true);
                UpdateInventoryUI();
            }
        }
    }

    void UpdateInventoryUI() {
        Transform list = transform.GetChild(0).GetChild(0).GetChild(0);
        foreach(Transform child in list)
            Destroy(child.gameObject);

        for(int i = 0; i < itemInv.getStackCount(); i++){
            ItemStack stack = itemInv.getStackAtIndex(i);
            GameObject go = Instantiate(InventoryElement, list);
            go.transform.Find("Name").GetComponent<Text>().text = stack.getItem().getName() + (stack.getCount() > 1 ? " (x" + stack.getCount() + ")" : "");
            go.transform.Find("Description").GetComponent<Text>().text = stack.getItem().getDescription();
            go.transform.Find("Image").GetComponent<Image>().sprite = stack.getItem().getSprite();
        }
    }

    public void addStack(ItemStack stack){
        itemInv.addStack(stack);
    }
}
