using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public string skillName;
    public int skillNum;
    public int level;
    public long mPc;
    public long price;

    public Skill(string skillName, int skillNum, int level, long mPc, int price)
    {
        this.skillName = skillName;
        this.skillNum = skillNum;
        this.level = level;
        this.mPc = mPc;
        this.price = price;
    }
}
