using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Consume : MonoBehaviour
{
    public Text box0;
    public Text box1;
    public Text box2;
    public Text box3;
    public Text bigBox;
    private int spot;
    private bool swap = false;
    private PlayerScript ps;
    private Ability take = null;
    private int give = -1;
    // Start is called before the first frame update
    void Start()
    {
        box0.text = "";
        box1.text = "";
        box2.text = "";
        box3.text = "";
        bigBox.text = "Will you consume the enemy, \n or allow the void to take him? ";
        spot = 0;
        ps = GameObject.FindGameObjectsWithTag("PlayerData")[0].GetComponent<PlayerScript>();
    }

    void clearAll() {
        box0.text = "";
        box1.text = "";
        box2.text = "";
        box3.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(spot == 0 && Input.GetMouseButtonDown(0)) {
            box0.text = "CONSUME";
            box1.text = "Leave";
        }
        if(spot == 1 && swap) {
            bigBox.text = "Very well, what will you be taking?";
            setAbilityText(ps.enemy);
            swap = false;
        }
        if(spot == 2 && swap) {
            bigBox.text = "Feeling merciful today are we?";
            spot = 10;
        }
        if(take != null) {
            if(ps.player.MoveList.Count >= 4) {
                bigBox.text = "You will be taking \n" +
                take.Name + "\n but what will you give up for it?";
                swap = false;
                spot = 3;
                setAbilityText(ps.player);
            }
            else {
                clearAll();
                bigBox.text = "Very Well.\n" + "You have taken "
                    + ps.enemy.Name + "'s " + take.Name;
                //Add the new move
                ps.player.MoveList.Add(take);
                //END
                spot = 10;
            }
        }
        if (give != -1) {
            bigBox.text = "Consumption successful. \n You now posses these moves.";
            //Give us the move.
            ps.player.MoveList[give] = take;
            setAbilityText(ps.player);
            spot = 10;
        }

        //END
        if(spot == 10 && Input.GetMouseButtonDown(0)) {
            SceneManager.LoadScene("TestScene");
        }
    }

    public void b0Press() {
        //Picked consume
        if(spot == 0) {
            spot = 1;
            swap = true;
            return;
        }
        //Choosing an ability to take
        if (spot == 1) {
            swap = true;
            take = ps.enemy.MoveList[0];
        }
        if (spot == 3) {
            swap = true;
            give = 0;
        }
    }
    public void b1Press() {
        //Picked level up
        if (spot == 0) {
            spot = 2;
            swap = true;
            return;
        }
        //Choosing an ability to take
        if (spot == 1 && ps.enemy.MoveList.Count > 1) {
            swap = true;
            take = ps.enemy.MoveList[1];
        }
        if (spot == 3 && ps.player.MoveList.Count > 1) {
            swap = true;
            give = 1;
        }
    }

    public void b2Press() {
        //Choosing an ability to take
        if (spot == 1 && ps.enemy.MoveList.Count > 2) {
            swap = true;
            take = ps.enemy.MoveList[2];
        }
        if (spot == 3 && ps.player.MoveList.Count > 2) {
            swap = true;
            give = 2;
        }
    }

    public void b3Press() {
        //Choosing an ability to take
        if (spot == 1 && ps.enemy.MoveList.Count > 3) {
            swap = true;
            take = ps.enemy.MoveList[3];
        }
        if (spot == 3 && ps.player.MoveList.Count > 3) {
            swap = true;
            give = 3;
        }
    }

    void setAbilityText(Nalp n) {
        box0.text = "";
        box1.text = "";
        box2.text = "";
        box3.text = "";
        for (int i = 0; i < n.MoveList.Count; i++) {
            switch (i) {
                case 0:
                    box0.text = n.MoveList[0].Name;
                    break;
                case 1:
                    box1.text = n.MoveList[1].Name;
                    break;
                case 2:
                    box2.text = n.MoveList[2].Name;
                    break;
                case 3:
                    box3.text = n.MoveList[3].Name;
                    break;
                default:
                    break;
            }
        }
    }
}
