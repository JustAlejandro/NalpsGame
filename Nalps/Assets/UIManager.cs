using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public BattleManager battleManager;
    private BattleData mostRecent;
    private int winner = -1;
    public ParticleSystem spray;
    private int state = 0;
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
            case 0:
                playerMoveWait();
                break;
            //Show enemy takes damage
            case 1:
                enemyTakeHit();
                break;
            //Show enemy attack
            case 2:
                enemyGiveHit();
                break;
            //Show player taking damage
            case 3:
                playerTakeHit();
                break;
            //Show if someone won
            case 4:
                showWinner();
                break;
            //End battle after showWinner ends
            case 5:
                endBattle();
                break;
            default:
                Debug.Log("FSM Broke");
                break;
        }
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
            state++;
        }
        setHp();
        checkWinner();
    }

    private void enemyGiveHit() {
        mostRecent = battleManager.takeMove();
        state++;
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
            state = 0;
        }
        setHp();
        checkWinner();
    }

    private void checkWinner() {
        if(playerHP <= 0) {
            winner = 1;
            state = 4;
        }
        else if(enemyHP <= 0) {
            winner = 0;
            state = 4;
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
            state = 5;
        }
    }

    private void endBattle() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    }

    private void setNames() {
        playerName.text = battleManager.battleInfo.names[0];
        enemyName.text = battleManager.battleInfo.names[1];
    }

    void setAbilityText() {
        for(int i = 0; i < battleManager.abilityInfo.moves.Count; i++) {
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
        if(state == 0) {
            if (id > battleManager.abilityInfo.moves.Count - 1) return;
            //Later using the returns here to show battle info
            mostRecent = battleManager.useMove(id);
            state++;
            change = true;
        }
    }
}
