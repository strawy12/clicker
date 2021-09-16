using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public int skillNum;
    public int level;
    public long price;
    public bool isSold;

    public Skill(string skillName, int skillNum, int level, int price)
    {
        this.skillName = skillName;
        this.skillNum = skillNum;
        this.level = level;
        this.price = price;
    }
}
