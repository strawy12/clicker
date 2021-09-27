
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public int skillNum;
    public int level;
    public int price;
    public bool isSold;
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
            return (int)Mathf.Max((baseCoolTime - (level * 1.25f)), 10);
        }
    }
    public bool isUsed;

    public string endTime;
    public string endDurationTime;

    public Skill(string skillName, int skillNum, int level, int price, int baseDuration, int baseCoolTime)
    {
        this.skillName = skillName;
        this.skillNum = skillNum;
        this.level = level;
        this.price = price;
        this.baseDuration = baseDuration;
        this.baseCoolTime = baseCoolTime;
    }

    public long PriceSum(int multiple)
    {
        long priceSum = this.price;
        long price = this.price;
        for(int i = 0; i < multiple; i++)
        {
            price += 10;
            priceSum += price;
        }
        return priceSum;
    }
}
