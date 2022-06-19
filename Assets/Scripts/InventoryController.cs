using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryController : MonoBehaviour {

    ItemInventory itemInv = new ItemInventory();
    public static InventoryController _instance;
    public GameObject InventoryCanvas;
    public GameObject InventoryElement;
    public GameObject ItemSpawnLocation;
    public GameObject ItemPickup;
    public GameObject InventoryAction;
    PlayerController player;
    List<GameObject> buttons = new List<GameObject>();
    ItemStack selected;
    int selectedIndex = 9999;
    public AudioClip uiClick;
    AudioSource audio;

    void Start() {
        if(_instance != null)
            Destroy(this.gameObject);
        else
            _instance = this;

        player = FindObjectOfType<PlayerController>();
        audio = player.GetComponentInChildren<AudioSource>();
    }

    void Update() {
        /*if (Input.GetButtonDown("Jump")) {
            GameObject go = Instantiate(ItemPickup, ItemSpawnLocation.transform.position, ItemSpawnLocation.transform.rotation);
            Item item = null;
            switch(UnityEngine.Random.Range(0,4)){
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
                go.GetComponent<ItemPickup>().setItem(new ItemStack(item, UnityEngine.Random.Range(1, 31)));
            }
        }*/

        /*if(Input.GetButtonDown("Fire1")){
            Debug.Log(player.getEquipedWeapon().getName());
        }*/

        if(Input.GetButtonDown("Inventory")){
            if(InventoryCanvas.activeSelf){
                InventoryCanvas.SetActive(false);
                player.unblockInput();
            }else{
                InventoryCanvas.SetActive(true);
                UpdateInventoryUI();
                player.blockInput();
            }
        }
    }

    void UpdateInventoryUI() {
        Transform list = transform.GetChild(0).GetChild(0).GetChild(0);
        foreach(Transform child in list)
            Destroy(child.gameObject);
        buttons = new List<GameObject>();

        for(int i = 0; i < itemInv.getStackCount(); i++){
            ItemStack stack = itemInv.getStackAtIndex(i);
            GameObject go = Instantiate(InventoryElement, list);
            buttons.Add(go);
            go.transform.Find("Name").GetComponent<Text>().text = stack.getItem().getName() + (stack.getCount() > 1 ? " (x" + stack.getCount() + ")" : "");
            go.transform.Find("Description").GetComponent<Text>().text = stack.getItem().getDescription();
            go.transform.Find("Image").GetComponent<Image>().sprite = stack.getItem().getSprite();
            int index = i;
            go.GetComponent<Button>().onClick.AddListener(() => { onClick(go, stack, index); });
            if(i == selectedIndex) go.GetComponent<Button>().OnSelect(null);
        }
    }

    public void addStack(ItemStack stack){
        itemInv.addStack(stack);
    }

    public PlayerController getPlayer(){
        return player;
    }

    void onClick(GameObject go, ItemStack stack, int Index){
        selected = stack;
        selectedIndex = Index;
        buttons.ForEach( obj => obj.GetComponent<Button>().OnDeselect(null));
        go.GetComponent<Button>().OnSelect(null);

        Transform list = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0);
        foreach(Transform child in list)
            Destroy(child.gameObject);

        Dictionary<string, Action<ItemStack>> actions = selected.getItem().getItemActions(new Dictionary<string, Action<ItemStack>>());

        foreach(string action in actions.Keys){
            GameObject obj = Instantiate(InventoryAction, list);
            obj.transform.Find("Name").GetComponent<Text>().text = action;
            obj.GetComponent<Button>().onClick.AddListener(() => { PlayClip(uiClick); actions[action](stack); UpdateInventoryUI(); });
        }
        PlayClip(uiClick);
    }

    public void dropStack(ItemStack stack){
        ItemStack drop = itemInv.removeStack(stack);
        if(drop.getCount() > 0){
            GameObject go = Instantiate(ItemPickup, player.transform.position, player.transform.rotation);
            go.GetComponent<ItemPickup>().setItem(drop);
        }
    }

    public void destroyStack(ItemStack stack){
        itemInv.removeStack(stack);
    }

    public void deselect(){
        selected = null;
        selectedIndex = 9999;
        Transform list = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0);
        foreach(Transform child in list)
            Destroy(child.gameObject);
    }

    void PlayClip(AudioClip clip){
        audio.clip = clip;
        audio.Play();
    }
}
