using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * FEATURES TO ADD:
 * 
 * Level Up (XP Gain)
 */
enum Types {
    Typeless,
    Fire,
    Water,
    Grass
}

enum Status {
    Poison,
    Paralyzed,
    Burning
}

[System.Serializable]
//This is the class that will be the basis for all Nalps, player and enemy alike
public class Nalp
{
    protected int hp;
    protected int maxHp;
    protected int xp;
    protected int level;
    protected int type;
    protected int speed;
    protected int strength;
    protected int resistance;
    protected List<bool> statusEffects;
    protected List<int> statusDuration;
    protected List<bool> statusResist;
    protected string nalpname;
    protected List<Ability> moveList;
    protected List<Item> items;
    private Nalp enemy;
    protected PlayerScript ps;

    public int Hp { get => hp; set => hp = value; }
    public int Xp { get => xp; set => xp = value; }
    public int Level { get => level; set => level = value; }
    public int Type { get => type; set => type = value; }
    public int Speed { get => speed; set => speed = value; }
    public int Strength { get => strength; set => strength = value; }
    public int Resistance { get => resistance; set => resistance = value; }
    public List<Ability> MoveList { get => moveList; set => moveList = value; }
    public string Name { get => nalpname; }
    protected List<bool> StatusEffects { get => statusEffects; }
    protected List<int> StatusDuration { get => statusDuration; set => statusDuration = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public List<bool> StatusResist { get => statusResist; set => statusResist = value; }
    public Nalp Enemy { get => enemy; set => enemy = value; }
    public List<Item> Items { get => items; set => items = value; }

    //Default Nalp Constructor
    public Nalp() {
        ps = PlayerScript.Instance;

        Hp = 10;
        maxHp = 10;
        Xp = 0;
        Level = 1;
        resistance = 0;
        strength = 1;
        speed = 1;

        MoveList = new List<Ability>();
        statusEffects = new List<bool>();
        statusDuration = new List<int>();
        statusResist = new List<bool>();
        items = new List<Item>();
        //Init array of status effects, which will all be false
        statusEffects = new List<bool>();
        for(int i = 0; i < Status.GetNames(typeof(Status)).Length; i++) {
            statusEffects.Add(false);
            statusDuration.Add(0);
            statusResist.Add(false);
        }

        //Default Nalp only knows tackle
        MoveList.Add(new Ability(ps.randomAbilityData()));
        nalpname = "Steve-O";
        Enemy = null;
    }

    //Give the nalp a target for combat
    public void enterCombat(Nalp e) {
        Enemy = e;
    }

    public ItemData useItem(int id) {
        items[id].useItem();
        ItemData iDat = items[id].ItemData;
        if(items[id].count < 1) {
            items.RemoveAt(id);
        }
        hp = Math.Min(hp +iDat.heal, maxHp);
        speed += iDat.speed;
        resistance += iDat.resistance;
        strength += iDat.strength;
        return iDat;
    }

    public bool giveItem(string s, int count = 1) {
        return giveItem(ps.GetItemData("Health Pot"), count);
    }

    public bool giveItem(ItemData itemData, int count = 1) {
        for(int i = 0; i < items.Count; i++) {
            if (itemData.Equals(items[i])) {
                items[i].count+= count;
                return true;
            }
        }
        if(items.Count >= 4) {
            return false;
        }
        items.Add(new Item(itemData));
        items[items.Count - 1].count += count;
        return false;
    }

    //Takes in the move to use on enemy
    public virtual BattleData useAbility(int index) {
        BattleData slapCity = MoveList[index].use(this);
        Enemy.recieveHit(slapCity);
        return slapCity;
    }

    public BattleData recieveHit(BattleData take) {
        if (take.hit == false) return take;
        //checking for status effects
        for(int i = 0; i < take.status.Count; i++) {
            if(take.status[i] > 0) {
                if (!statusResist[i]) {
                    statusDuration[i] = Math.Max(statusDuration[i], take.status[i]);
                    statusEffects[i] = true;
                }
            }
        }

        //Deal damage
        take.damage = take.damage * (Convert.ToInt32(take.crit) + 1);
        int damage = take.damage;
        hp = hp - Math.Max(take.damage - resistance, 0);
        updateStatus();
        return take;
    }

    public float hpPercent() {
        return ((float)Hp) / ((float)MaxHp);
    }

    protected void updateStatus() {
        poison();
        paralyze();
        burning();
    }

    //Poison reduces HP by 1/8 every turn until it runs out
    private void poison() {
        if (statusEffects[(int)Status.Poison]) {
            statusDuration[(int)Status.Poison]--;
            if (statusDuration[(int)Status.Poison] > 0) {
                hp = hp - (MaxHp / 8);
            }
            else {
                statusEffects[(int)Status.Poison] = false;
            }
        }
    }

    //Para stops movement until runs out
    private void paralyze() {
        if (statusEffects[(int)Status.Paralyzed]){
            statusDuration[(int)Status.Paralyzed]--;
            if (!(statusDuration[(int)Status.Paralyzed] > 0)) {
                statusEffects[(int)Status.Paralyzed] = false;
            }
        }
    }

    //Burning reduces hp by 1/5
    private void burning() {
        if (statusEffects[(int)Status.Burning]) {
            statusDuration[(int)Status.Burning]--;
            if (statusDuration[(int)Status.Burning] > 0) {
                hp = hp - (MaxHp / 5);
            }
            else {
                statusEffects[(int)Status.Burning] = false;
            }
        }
    }
}

public class Player : Nalp {
    public Player() {
        ps = PlayerScript.Instance;

        Hp = 10;
        maxHp = 10;
        Xp = 0;
        Level = 1;
        resistance = 0;
        strength = 1;
        speed = 1;
        MoveList = new List<Ability>();
        statusEffects = new List<bool>();
        statusDuration = new List<int>();
        statusResist = new List<bool>();
        //Init array of status effects, which will all be false
        statusEffects = new List<bool>();
        for (int i = 0; i < Status.GetNames(typeof(Status)).Length; i++) {
            statusEffects.Add(false);
            statusDuration.Add(0);
            statusResist.Add(false);
        }
        items = new List<Item>();

        MoveList.Add(new Ability(ps.GetAbilityData("Tackle")));
        MoveList.Add(new Ability(ps.GetAbilityData("Slap")));
        MoveList.Add(new Ability(ps.GetAbilityData("Crush")));

        MoveList.Add(new Ability(ps.GetAbilityData("Fire Ball")));

        nalpname = "Richard";
        Enemy = null;
    }

    public override BattleData useAbility(int index) {
        BattleData slapCity = MoveList[index].use(this);
        Enemy.recieveHit(slapCity);
        if(moveList[index].CurUse <= 0) {
            moveList.RemoveAt(index);
        }
        return slapCity;
    }
}
