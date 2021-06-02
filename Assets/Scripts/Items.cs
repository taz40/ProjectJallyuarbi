using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items {

    //Items
    public static Item ITEM_TWIG = new Item("item:twig");
    public static Item ITEM_BERRY = new Item("item:berry");
    
    //Weapons
    public static ItemWeapon WEAPON_SWORD = new ItemWeapon("item:sword", 10, 2);

    //Key Items
    public static ItemKey KEY_ITEM_GATE_KEY = new ItemKey("item:gate_key");
}
