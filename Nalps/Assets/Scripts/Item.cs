using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData {
    public string name;
    public string desc;
    public int heal;
    public int resistance;
    public int speed;
    public int strength;
    public ItemData(string s, string d, int h, int r, int sp, int str) {
        name = s;
        desc = d;
        heal = h;
        resistance = r;
        speed = sp;
        strength = str;
    }
}
public class Item
{
    public string name;
    public string desc;
    public int count;
    public int heal;
    public int resistance;
    public int speed;
    public int strength;

    public Item(Item item) {
        name = item.name;
        desc = item.desc;
        count = 1;
        heal = item.heal;
        resistance = item.resistance;
        speed = item.speed;
        strength = item.strength;
    }

    public Item(string s = "", string d = "", int c = 0, int h = 0, int res = 0, int speed = 0, int str = 0) {
        name = s;
        desc = d;
        count = c;
        heal = h;
        resistance = res;
        this.speed = speed;
        strength = str;
    }
    public ItemData Use() {
        count--;
        return new ItemData(name, desc, heal, resistance, speed, strength);
    }
}
