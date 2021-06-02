using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

    string name;

    public Item(string name) {
        this.name = name;
    }

    public string getName() {
        return name;
    }

    public virtual int getMaxStackSize() {
        return int.MaxValue;
    }

}
