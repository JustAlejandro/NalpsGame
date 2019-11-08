using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public TextAsset ItemData;
    public Dictionary<string, Item> allItems = new Dictionary<string, Item>();
    public Player player;
    public Nalp enemy;
    public Vector3 position = new Vector3(0.475f, -1.513f);
    // Start is called before the first frame update
    void Start()
    {
        player = new Player();
        enemy = new Nalp();
        //Parse the Items
        string text = ItemData.text;
        text = text.Replace("\t", "");
        text = text.Replace("\r", "");
        string[] inputs = text.Split('\n');
        for(int i = 1; i < inputs.Length; i++) {
            string n = inputs[i];
            i++;
            string desc = inputs[i];
            i++;
            int speed = 0;
            int strength = 0;
            int heal = 0;
            int resistance = 0;
            while (!inputs[i].Contains("~")) {
                int cur = Int32.Parse(Regex.Match(inputs[i], @"\d+").Value);
                if (inputs[i].Contains("Speed")) {
                    speed = cur;
                }
                if (inputs[i].Contains("Strength")) {
                    strength = cur;
                }
                if (inputs[i].Contains("Heal")) {
                    heal = cur;
                }
                if (inputs[i].Contains("Resistance")) {
                    resistance = cur;
                }
                i++;
            }
            i++;
            while (i < inputs.Length && !inputs[i].Contains("~")) {
                i++;
            }
            allItems.Add(n, new Item(n, desc, 0, heal, resistance, speed, strength));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Stop from being deleted on new scene
    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PlayerData");

        if (objs.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

}
