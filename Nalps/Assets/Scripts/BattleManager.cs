using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private Player player;
    private Nalp enemy;
    public BattleInfo battleInfo;
    public AbilityInfo abilityInfo;
    // Start is called before the first frame update
    void Start()
    {
        //find the player object by reference
        player = GameObject.FindGameObjectsWithTag("PlayerData")[0].GetComponent<PlayerScript>().player;
        enemy = new Nalp();

        //Manually set enemies
        player.Enemy = enemy;
        enemy.Enemy = player;

        //Create info for UI
        battleInfo = new BattleInfo(player, enemy);
        abilityInfo = new AbilityInfo(player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Nalp getEnemy() {
        return enemy;
    }

    public List<Item> GetItems() {
        return player.Items;
    }

    public ItemData useItem(int id) {
        return player.useItem(id);
    }

    //id == 0 (player) else enemy
    public float hpPercent(int id) {
        if(id == 0) {
            return player.hpPercent();
        }
        else {
            return enemy.hpPercent();
        }
    }

    //UI will ask us to use a move.
    public BattleData useMove(int i) {
        if(i > abilityInfo.moves.Count) {
            throw new System.InvalidOperationException("MOVE RANGE EXCEEDED");
        }
        BattleData toRet = player.useAbility(i);
        updateUI();
        return toRet;
    }
    
    public BattleData takeMove(int i = 0) {
        BattleData toRet = enemy.useAbility(0);
        updateUI();
        return toRet;
    }

    public void updateUI() {
        battleInfo.update(player, enemy);
        abilityInfo.update(player);
    }
}

public class BattleInfo {
    public List<int> hpCur;
    public List<int> hpTot;
    public List<string> names;
    public BattleInfo(Player player, Nalp n) {
        hpCur = new List<int>();
        hpTot = new List<int>();
        names = new List<string>();
        hpCur.Add(player.Hp);
        hpCur.Add(n.Hp);
        hpTot.Add(player.MaxHp);
        hpTot.Add(n.MaxHp);
        names.Add(player.Name);
        names.Add(n.Name);
    }

    public void update(Player player, Nalp n) {
        hpCur.Clear();
        hpTot.Clear();
        names.Clear();
        hpCur.Add(player.Hp);
        hpCur.Add(n.Hp);
        hpTot.Add(player.MaxHp);
        hpTot.Add(n.MaxHp);
        names.Add(player.Name);
        names.Add(n.Name);
    }
}

public class AbilityInfo {
    public List<Ability> moves;
    public AbilityInfo(Player player) {
        moves = new List<Ability>();
        foreach (Ability a in player.MoveList) {
            moves.Add(a);
        }
    }

    public void update(Player player) {
        moves.Clear();
        foreach (Ability a in player.MoveList) {
            moves.Add(a);
        }
    }
}
