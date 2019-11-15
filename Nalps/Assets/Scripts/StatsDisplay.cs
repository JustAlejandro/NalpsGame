using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatsDisplay: MonoBehaviour
{
    public Text HPLabel;
    public Text StrengthLabel;
    public Text ResistanceLabel;
    public Text SpeedLabel;
    public Text LevelLabel;

    public void setLabels(int hp,int strength,int resistance,int speed, int level)
    {
        HPLabel.text = hp.ToString();
        StrengthLabel.text = strength.ToString();
        ResistanceLabel.text = resistance.ToString();
        SpeedLabel.text = speed.ToString();
        LevelLabel.text = level.ToString();

    }
}
