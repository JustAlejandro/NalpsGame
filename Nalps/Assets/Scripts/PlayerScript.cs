using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public List<ItemData> itemDatas;
    public Player player;
    public Nalp enemy;
    public Vector3 position = new Vector3(0.475f, -1.513f);
    // Start is called before the first frame update
    void Start() {
        itemDatas = new List<ItemData>(Resources.LoadAll<ItemData>("Items"));
        player = new Player();
        enemy = new Nalp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ItemData GetItemData(string n) {
        foreach(ItemData id in itemDatas) {
            if (id.Name.Equals(n)) {
                return id;
            }
        }
        return null;
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
