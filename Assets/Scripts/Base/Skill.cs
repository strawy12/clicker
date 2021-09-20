
using System.Collections.Generic;
using UnityEngine;
using ESkillSType = GameManager.ESkillType;

[System.Serializable]
public class Skill
{
    public string skillName;
    public int skillNum;
    public int level;
    public long price;
    public bool isSold;
    public ESkillSType skilltype;
    public int baseCoolTime;
    public int baseDuration;
    public int duration
    {
        get
        {
            return (int)(baseDuration + (level * 1.25f)) * Mathf.Min(1, baseDuration);
        }
    }
    public int coolTime
    {
        get
        {
            return (int)(baseCoolTime - (level * 1.25f));
        }
    }
    public bool isUsed;

    public string endTime;
    public string endDurationTime;

    public Skill(string skillName, int skillNum, int level, int price, int baseDuration, int baseCoolTime,  ESkillSType skilltype)
    {
        this.skillName = skillName;
        this.skillNum = skillNum;
        this.level = level;
        this.price = price;
        this.baseDuration = baseDuration;
        this.baseCoolTime = baseCoolTime;
        this.skilltype = skilltype;
    }
}
