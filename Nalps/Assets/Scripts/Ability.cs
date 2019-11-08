using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]

//BattleData holds the response to the attack
public class BattleData {
    //Did move hit
    public bool hit;
    //Did move crit?
    public bool crit;
    //Was this a favorable type matchup?
    public float multiplier;

    public int damage;

    //Type and name of the Move(for UI use)
    public int type;
    public string name;
    public List<int> status;

    public BattleData(bool h = false, bool c = false, int d = 0, int t = (int)Types.Typeless, 
        string n = "NAMELESS MOVE", List<int> stats = null) {
        hit = h;
        crit = c;
        damage = d;
        type = t;
        name = n;
        //Make something if null
        stats = stats ?? new List<int>();
        if(stats.Count() <= 0) {
            status = Enumerable.Repeat(0, Status.GetNames(typeof(Status)).Length).ToList();
        }
        else {
            status = stats;
        }
    }
}

//All attacks and other stuff in handled here, with the different types of moves being subclasses
//Not the ideal setup, but not terrible either
public class Ability
{
    protected int maxUse;
    protected int curUse;
    protected int damage;
    protected int type;
    protected float accuracy;
    protected float critChance;
    protected string name;
    public List<int> status = null;

    public Ability() {
        maxUse = 5;
        curUse = 5;
        damage = 5;
        type = (int)Types.Typeless;
        accuracy = 1.0f;
        critChance = 0.0f;
        name = "DEFAULT MOVE";
    }

    //Return the BattleData to be read by the enemy
    //Input: Nalp to modify the move if the user's stats influence its power
    public virtual BattleData use(Nalp user) {
        curUse--;
        return new BattleData(Hit(), Crit(), damage, type, name);
    }

    //Check if a move hit/crit
    protected bool Hit() {
        return Random.value < accuracy;
    }
    protected bool Crit() {
        return Random.value < critChance;
    }

    //Getters for Ability field stuff
    public int CurUse { get => curUse; }
    public int MaxUse { get => maxUse; }
    public int Damage { get => damage; }
    public int Type { get => type; }
    public float Accuracy { get => accuracy; }
    public float CritChance { get => critChance; }
    public string Name { get => name; }
}

public class Tackle : Ability {
    public Tackle() {
        maxUse = 10;
        curUse = 10;
        damage = 1;
        type = (int)Types.Typeless;
        accuracy = 1.0f;
        critChance = 0.1f;
        name = "Tackle";
    }
}

public class FireBall : Ability {
    public FireBall() {
        maxUse = 5;
        curUse = 5;
        damage = 3;
        type = (int)Types.Fire;
        accuracy = 0.8f;
        critChance = 0.05f;
        name = "Fire Ball";
    }

    public override BattleData use(Nalp user) {
        int d = damage + user.Strength;
        curUse--;
        return new BattleData(Hit(), Crit(), d, type, name);
    }
}

public class DEATH : Ability {
    public DEATH() {
        maxUse = 1;
        curUse = 1;
        damage = 9999999;
        type = (int)Types.Typeless;
        accuracy = 1.0f;
        critChance = 0.0f;
        name = "I AM BECOME DEATH DESTROYER OF WORLDS";
    }
}

public class Wait : Ability {
    public Wait() {
        maxUse = 10;
        curUse = 10;
        damage = 0;
        type = (int)Types.Typeless;
        accuracy = 1.0f;
        critChance = 1.0f;
        name = "Take a Nap";
    }
}