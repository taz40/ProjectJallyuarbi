using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKey : Item {

    public ItemKey(string name) 
        : base(name) {

    }

    public override int getMaxStackSize() {
        return 1;
    }

}
