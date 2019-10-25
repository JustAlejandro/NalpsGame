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
    private Nalp enemy;

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

    //Default Nalp Constructor
    public Nalp() {
        Hp = 10;
        maxHp = 10;
        Xp = 0;
        Level = 1;
        resistance = 0;
        MoveList = new List<Ability>();
        statusEffects = new List<bool>();
        statusDuration = new List<int>();
        statusResist = new List<bool>();
        //Init array of status effects, which will all be false
        statusEffects = new List<bool>();
        for(int i = 0; i < Status.GetNames(typeof(Status)).Length; i++) {
            statusEffects.Add(false);
            statusDuration.Add(0);
            statusResist.Add(false);
        }

        //Default Nalp only knows tackle
        MoveList.Add(new Tackle());
        nalpname = "Steve-O";
        Enemy = null;
    }

    //Give the nalp a target for combat
    public void enterCombat(Nalp e) {
        Enemy = e;
    }

    //Takes in the move to use on enemy
    public BattleData useAbility(int index) {
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
                statusEffects[(int)Status.Poison] = false;
            }
        }
    }

    //Burning reduces hp by 1/5
    private void burning() {
        if (statusEffects[(int)Status.Poison]) {
            statusDuration[(int)Status.Poison]--;
            if (statusDuration[(int)Status.Poison] > 0) {
                hp = hp - (MaxHp / 5);
            }
            else {
                statusEffects[(int)Status.Poison] = false;
            }
        }
    }
}

public class Player : Nalp {
    public Player() {
        Hp = 10;
        maxHp = 10;
        Xp = 0;
        Level = 1;
        resistance = 0;
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

        MoveList.Add(new Tackle());
        MoveList.Add(new FireBall());
        MoveList.Add(new DEATH());
        nalpname = "Richard";
        Enemy = null;
    }
}
