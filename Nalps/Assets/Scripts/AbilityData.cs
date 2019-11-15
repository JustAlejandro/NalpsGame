using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class AbilityData : ScriptableObject {
    public string title;
    public int maxUse;
    public int damage;
    public int type;
    public float accuracy;
    public float critChance;
    public List<int> status;
}
