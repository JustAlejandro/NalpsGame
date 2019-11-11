using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

enum UIState {
    PLAYERMOVEWAIT = 0,
    ENEMYTAKEHIT = 1,
    ENEMYGIVEHIT = 2,
    PLAYERTAKEHIT = 3,
    SHOWWINNER = 4,
    ENDBATTLE = 5,
    PICKACTION = 6,
    PLAYERITEMWAIT = 7,
    PLAYERITEMUSE = 8,
    SHOWSTATS = 9,
    TRYRUN = 10
}

public class UIManager : MonoBehaviour {

    public Text box0;
    public Text box1;
    public Text box2;
    public Text box3;
    public Text hpPlayer;
    public Text hpEnemy;
    public Text playerName;
    public Text enemyName;
    public Text bigBox;
    public HealthBar pHP;
    public HealthBar eHP;
    public BattleManager battleManager;
    private BattleData mostRecent;
    private ItemData recentItem;
    private int winner = -1;
    public ParticleSystem spray;
    private int state = (int)UIState.PICKACTION;
    private bool change = true;
    private bool click = false;
    private int enemyHP;
    private int playerHP;
    private float countSpeed;
    private float countLeft;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        click = Input.GetMouseButtonDown(0);
        switch (state) {
            //Wait on Player
            case (int)UIState.PLAYERMOVEWAIT:
                playerMoveWait();
                break;
            //Show enemy takes damage
            case (int)UIState.ENEMYTAKEHIT:
                enemyTakeHit();
                break;
            //Show enemy attack
            case (int)UIState.ENEMYGIVEHIT:
                enemyGiveHit();
                break;
            //Show player taking damage
            case (int)UIState.PLAYERTAKEHIT:
                playerTakeHit();
                break;
            //Show if someone won
            case (int)UIState.SHOWWINNER:
                showWinner();
                break;
            //End battle after showWinner ends
            case (int)UIState.ENDBATTLE:
                endBattle();
                break;
            case (int)UIState.PICKACTION:
                pickAction();
                break;
            case (int)UIState.PLAYERITEMWAIT:
                playerItemWait();
                break;
            case (int)UIState.PLAYERITEMUSE:
                playerItemUse();
                break;
            default:
                Debug.Log("FSM Broke");
                break;
        }
    }

    private void playerItemUse() {
        if (change) {
            change = false;
            string set = "You used " + recentItem.Name + " and gained ";
            if (recentItem.heal > 0) set += recentItem.heal + "HP!";
            if (recentItem.speed > 0) set += recentItem.speed + " Speed!";
            if (recentItem.strength > 0) set += recentItem.strength + " Strength!";
            if (recentItem.resistance > 0) set += recentItem.resistance + " Resistance!";
            bigBox.text = set;
        }
        if (click) {
            state = (int)UIState.ENEMYGIVEHIT;
        }
    }

    private void playerItemWait() {
        bigBox.text = "Select an Item:";
        box0.text = "";
        box1.text = "";
        box2.text = "";
        box3.text = "";
        setAbilityText();
    }

    private void pickAction() {
        bigBox.text = "What will you do?";
        box0.text = "";
        box1.text = "";
        box2.text = "";
        box3.text = "";

        battleManager.updateUI();
        playerHP = battleManager.battleInfo.hpCur[0];
        enemyHP = battleManager.battleInfo.hpCur[1];
        setAbilityText();
        setNames();
        setHp();
    }

    private void playerMoveWait() {
        bigBox.text = "Choose Your Move:";
        box0.text = "";
        box1.text = "";
        box2.text = "";
        box3.text = "";
        battleManager.updateUI();
        playerHP = battleManager.battleInfo.hpCur[0];
        enemyHP = battleManager.battleInfo.hpCur[1];
        setAbilityText();
        setNames();
        setHp();

        countSpeed = 2.0f / (float)(battleManager.battleInfo.hpTot[0]);
        countLeft = countSpeed;
    }

    private void enemyTakeHit() {
        //Change text on new state
        if (change) {
            change = false;
            //Attack Hit
            if (mostRecent.hit) {
                bigBox.text = playerName.text + 
                    (mostRecent.crit ? " critically" : "") +
                    " hit enemy " + enemyName.text + " with " +
                    mostRecent.name +
                    " for " + mostRecent.damage + "HP!";
            }
            //Attack Miss
            else {
                bigBox.text = playerName.text + "'s " + mostRecent.name +
                    " missed!";
            }
        }
        if(enemyHP != battleManager.battleInfo.hpCur[1]) {
            countLeft -= Time.deltaTime;
            if(countLeft < 0) {
                countLeft = countSpeed;
                enemyHP--;
            }
        }
        if (click) {
            enemyHP = battleManager.battleInfo.hpCur[1];
            state = (int)UIState.ENEMYGIVEHIT;
        }
        setHp();
        checkWinner();
    }

    private void enemyGiveHit() {
        mostRecent = battleManager.takeMove();
        state = (int)UIState.PLAYERTAKEHIT;
        change = true;
    }

    private void playerTakeHit() {
        //Change text on new state
        if (change) {
            change = false;
            //Attack Hit
            if (mostRecent.hit) {
                bigBox.text = enemyName.text +
                    (mostRecent.crit ? " critically" : "") +
                    " hit " + playerName.text + " with " +
                    mostRecent.name +
                    " for " + mostRecent.damage + "HP!";
            }
            //Attack Miss
            else {
                bigBox.text = enemyName.text + "'s " + mostRecent.name +
                    " missed!";
            }
        }
        if (playerHP != battleManager.battleInfo.hpCur[0]) {
            countLeft -= Time.deltaTime;
            if (countLeft < 0) {
                countLeft = countSpeed;
                playerHP--;
            }
        }
        if (click) {
            playerHP = battleManager.battleInfo.hpCur[0];
            state = (int)UIState.PLAYERMOVEWAIT;
        }
        setHp();
        checkWinner();
    }

    private void checkWinner() {
        if(playerHP <= 0) {
            winner = 1;
            state = (int)UIState.SHOWWINNER;
        }
        else if(enemyHP <= 0) {
            winner = 0;
            state = (int)UIState.SHOWWINNER;
        }
    }

    private bool winShow = false;
    private void showWinner() {
        if (click && !winShow) {
            if (winner == 0) {
                bigBox.text = playerName.text + " butchered " + enemyName.text + "!";
            }
            if (winner == 1) {
                bigBox.text = enemyName.text + " butchered " + playerName.text + "!";
            }
            winShow = true;
            click = false;
        }
        
        if (click && winShow) {
            state = (int)UIState.ENDBATTLE;
        }
    }

    private void endBattle() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Player won
        if(winner == 0) {
            //Set the enemy in playerData for consumption?
            GameObject.FindGameObjectsWithTag("PlayerData")[0].GetComponent<PlayerScript>().enemy = 
                battleManager.getEnemy();
            SceneManager.LoadScene("Consume");
        }
        //Enemy won
        else {
            SceneManager.LoadScene("END");
        }
    }

    private void setHp() {
        if (playerHP <= 0) {
            winner = 1;
            if (!spray.isPlaying) spray.Play();
        }
        else if (enemyHP <= 0) {
            winner = 0;
            if (!spray.isPlaying) spray.Play();
        }
        hpPlayer.text = playerHP + "/" +
            battleManager.battleInfo.hpTot[0];
        hpEnemy.text = enemyHP + "/" +
                    battleManager.battleInfo.hpTot[1];
        pHP.UpdateHP(playerHP, battleManager.battleInfo.hpTot[0]);
        eHP.UpdateHP(enemyHP, battleManager.battleInfo.hpTot[1]);
    }

    private void setNames() {
        playerName.text = battleManager.battleInfo.names[0];
        enemyName.text = battleManager.battleInfo.names[1];
    }

    void setAbilityText() {
        box0.text = "";
        box1.text = "";
        box2.text = "";
        box3.text = "";
        if(state == (int)UIState.PICKACTION) {
            box0.text = "FIGHT";
            box1.text = "ITEMS";
            box2.text = "STATS";
            box3.text = "RUN";
        }
        if (state == (int)UIState.PLAYERMOVEWAIT) {
            for (int i = 0; i < battleManager.abilityInfo.moves.Count; i++) {
                switch (i) {
                    case 0:
                        box0.text = battleManager.abilityInfo.moves[0].Name;
                        break;
                    case 1:
                        box1.text = battleManager.abilityInfo.moves[1].Name;
                        break;
                    case 2:
                        box2.text = battleManager.abilityInfo.moves[2].Name;
                        break;
                    case 3:
                        box3.text = battleManager.abilityInfo.moves[3].Name;
                        break;
                    default:
                        break;
                }
            }
        }
        if (state == (int)UIState.PLAYERITEMWAIT) {
            List<Item> items = battleManager.GetItems();
            for (int i = 0; i < items.Count; i++) {
                switch (i) {
                    case 0:
                        box0.text = items[0].ItemData.Name;
                        break;
                    case 1:
                        box1.text = items[1].ItemData.Name;
                        break;
                    case 2:
                        box2.text = items[2].ItemData.Name;
                        break;
                    case 3:
                        box3.text = items[3].ItemData.Name;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void b1Hit() {
        abilitySelect(0);
    }
    public void b2Hit() {
        abilitySelect(1);
    }
    public void b3Hit() {
        abilitySelect(2);
    }
    public void b4Hit() {
        abilitySelect(3);
    }
    public void abilitySelect(int id) {
        if(state == (int)UIState.PLAYERMOVEWAIT) {
            if (id > battleManager.abilityInfo.moves.Count - 1) return;
            //Later using the returns here to show battle info
            mostRecent = battleManager.useMove(id);
            state++;
            change = true;
        }
        if (state == (int)UIState.PICKACTION) {
            switch (id) {
                case 0:
                    state = (int)UIState.PLAYERMOVEWAIT;
                    break;
                case 1:
                    state = (int)UIState.PLAYERITEMWAIT;
                    break;
                case 2:
                    //state = (int)UIState.SHOWSTATS;
                    break;
                case 3:
                    //state = (int)UIState.TRYRUN;
                    break;
                default:
                    break;
            }
        }
        if (state == (int)UIState.PLAYERITEMWAIT) {
            if (id > battleManager.GetItems().Count - 1) return;

            recentItem = battleManager.useItem(id);
            state = (int)UIState.PLAYERITEMUSE;
            change = true;
        }
    }
}
