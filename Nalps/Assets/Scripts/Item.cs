using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemData ItemData;
    public int count;

    public Item(ItemData itemData) {
        ItemData = itemData;
        count = 1;
    }
    //Gives the amount of uses left now
    public int useItem() {
        count--;
        return count;
    }

    public override bool Equals(object other) {
        Item item = other as Item;
        return item != null && item.ItemData.Equals(ItemData);
    }
}
