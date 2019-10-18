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
public class Nalp : MonoBehaviour
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
    private List<bool> statusResist;
    protected string nalpname;
    protected List<Ability> moveList;
    protected Nalp enemy;

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

    //Default Nalp Constructor
    public Nalp() {
        Hp = 10;
        maxHp = 10;
        Xp = 0;
        Level = 1;
        MoveList = new List<Ability>();
        //Init array of status effects, which will all be false
        statusEffects = new List<bool>();
        for(int i = 0; i < Status.GetNames(typeof(Status)).Length; i++) {
            statusEffects.Add(false);
            statusDuration.Add(0);
            statusResist.Add(false);
        }

        //Default Nalp only knows tackle
        MoveList.Add(new Tackle());
        nalpname = "Default Nalp";
        enemy = null;
    }

    //Give the nalp a target for combat
    public void enterCombat(Nalp e) {
        enemy = e;
    }

    //Takes in the move to use on enemy
    public void useAbility(int index) {
        BattleData slapCity = MoveList[index].use(this);
        enemy.recieveHit(slapCity);
    }

    public void recieveHit(BattleData take) {
        if (take.hit == false) return;
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
        int damage = take.damage * (Convert.ToInt32(take.crit) + 1);
        hp = hp - Math.Min(take.damage - resistance, 0);
        updateStatus();
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
