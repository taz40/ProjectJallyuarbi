using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKey : Item {

    public ItemKey(string id, string name, string description) 
        : base(id, name, description) {

    }

    public override int getMaxStackSize() {
        return 1;
    }

}
