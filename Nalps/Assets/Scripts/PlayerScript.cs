using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public List<ItemData> itemDatas;
    public List<AbilityData> abilityDatas;
    public Player player;
    public Nalp enemy;
    public Vector3 position;
    // Start is called before the first frame update
    void Start() {
        itemDatas = new List<ItemData>(Resources.LoadAll<ItemData>("Items"));
        abilityDatas = new List<AbilityData>(Resources.LoadAll<AbilityData>("Abilities"));
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

    public AbilityData randomAbilityData() {
        return abilityDatas[Mathf.RoundToInt(UnityEngine.Random.Range(0, abilityDatas.Count - 1))];
    }

    public AbilityData GetAbilityData(string n) {
        foreach(AbilityData ad in abilityDatas) {
            if (ad.title.Equals(n))
                return ad;
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

    #region singleton

    private static PlayerScript _instance;
    public static PlayerScript Instance => _instance ? _instance : (_instance = FindObjectOfType<PlayerScript>());
   
    #endregion

}
