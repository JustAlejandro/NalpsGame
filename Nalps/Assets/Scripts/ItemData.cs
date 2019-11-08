using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public string Name;
    public string description;

    public Sprite art;

    public int heal;
    public int resistance;
    public int speed;
    public int strength;

    public override bool Equals(object other) {
        ItemData id = other as ItemData;
        return id != null && Name.Equals(id.Name);
    }
}
