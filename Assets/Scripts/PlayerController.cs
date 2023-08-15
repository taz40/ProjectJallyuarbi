using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    PlayerMovement playerMovement;
    Interact interact;
    ItemWeapon equipedWeapon;
    ItemHelm equipedHelm;
    ItemChest equipedChest;
    ItemLeg equipedLeg;
    ItemBoot equipedBoot;

    // Start is called before the first frame update
    void Start() {
        playerMovement = GetComponent<PlayerMovement>();
        interact = GetComponent<Interact>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public ItemWeapon getEquipedWeapon(){
        return equipedWeapon;
    }

    public void equipWeapon(ItemWeapon weapon){
        equipedWeapon = weapon;
    }

    public ItemHelm getEquipedHelm(){
        return equipedHelm;
    }

    public void equipHelm(ItemHelm Helm){
        equipedHelm = Helm;
    }

    public ItemChest getEquipedChest(){
        return equipedChest;
    }

    public void equipChest(ItemChest Chest){
        equipedChest = Chest;
    }

    public ItemLeg getEquipedLeg(){
        return equipedLeg;
    }

    public void equipLeg(ItemLeg Leg){
        equipedLeg = Leg;
    }

    public ItemBoot getEquipedBoot(){
        return equipedBoot;
    }

    public void equipBoot(ItemBoot Boot){
        equipedBoot = Boot;
    }

    public void blockInput() {
        playerMovement.enabled = false;
        interact.enabled = false;
    }

    public void unblockInput() {
        playerMovement.enabled = true;
        interact.enabled = true;
    }

}
