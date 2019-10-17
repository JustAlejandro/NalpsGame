using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Types {
    Typeless,
    Fire,
    Water,
    Grass
}

[System.Serializable]
//This is the class that will be the basis for all Nalps, player and enemy alike
public class Nalp : MonoBehaviour
{
    protected int hp;
    protected int xp;
    protected int level;
    protected int type;
    protected int speed;
    protected int strength;
    protected int resistance;
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

    //Default Nalp Constructor
    public Nalp() {
        Hp = 10;
        Xp = 0;
        Level = 1;
        MoveList = new List<Ability>();
        //Default Nalp only knows tackle
        MoveList.Add(new Tackle());
        nalpname = "Default Nalp";
        enemy = null;
    }

    //Give the nalp a target for combat
    public void enterCombat(Nalp e) {
        enemy = e;
    }
}
