using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //If type == 0 this is player if type == 1 this is enemy
    public int type;
    public BattleManager bm;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateHP(int cur, int tot)
    {
        slider.value = ((float)cur) / ((float)tot);
    }
}
