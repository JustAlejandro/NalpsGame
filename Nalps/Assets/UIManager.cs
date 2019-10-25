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
    public Text text;
    public Text hpPlayer;
    public Text hpEnemy;
    public Text playerName;
    public Text enemyName;
    public Text bigBox;
    public BattleManager battleManager;
    private bool waiting;
    private int winner = -1;
    public ParticleSystem spray;

    // Start is called before the first frame update
    void Start() {
        waiting = false;
    }

    // Update is called once per frame
    void Update() {
        if (winner == -1) {
            text.text = "Choose Your Move:";
            box0.text = "";
            box1.text = "";
            box2.text = "";
            box3.text = "";
            battleManager.updateUI();
            setAbilityText();
            setNames();
            setHp();
        }
        if (winner == 0) {
            waiting = true;
            bigBox.text = playerName.text + " Beat " + enemyName.text + " to death.";
        }
        if(winner == 1) {
            waiting = true;
            bigBox.text = enemyName.text + " Beat " + playerName.text + " to death.";
        }
        if (waiting && Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void setHp() {
        if (battleManager.battleInfo.hpCur[0] <= 0) {
            winner = 1;
            if (!spray.isPlaying) spray.Play();
        }
        else if (battleManager.battleInfo.hpCur[1] <= 0) {
            winner = 0;
            if (!spray.isPlaying) spray.Play();
        }
            hpPlayer.text = battleManager.battleInfo.hpCur[0] + "/" +
            battleManager.battleInfo.hpTot[0];
        hpEnemy.text = battleManager.battleInfo.hpCur[1] + "/" +
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
        if (waiting) return;
        abilitySelect(0);
    }
    public void b2Hit() {
        if (waiting) return;
        abilitySelect(1);
    }
    public void b3Hit() {
        if (waiting) return;
        abilitySelect(2);
    }
    public void b4Hit() {
        if (waiting) return;
        abilitySelect(3);
    }
    public void abilitySelect(int id) {
        //Later using the returns here to show battle info
        battleManager.useMove(id);
        battleManager.takeMove();
    }
}
