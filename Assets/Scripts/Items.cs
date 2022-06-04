using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items {

    //Items
    public static Item ITEM_TWIG = new Item("item:twig", "Stick", "A simple stick");
    public static Item ITEM_BERRY = new Item("item:berry", "Berry", "Yummy Berries");
    
    //Weapons
    public static ItemWeapon WEAPON_SWORD = new ItemWeapon("item:sword", "Stone Sword", "I can swing my sword, sword.", 10, 2);

    //Key Items
    public static ItemKey KEY_ITEM_GATE_KEY = new ItemKey("item:gate_key", "Key", "A key to something, but what?");
}
