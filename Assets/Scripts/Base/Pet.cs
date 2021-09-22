using System.Numerics;
using UnityEngine;

[System.Serializable]
public class Pet
{
    public int petNum;
    public string petName;
    public int amount;
    public int maxAmount
    {
        get
        {
            return level * 2;
        }
    }
    public int level;
    public int percent;
    public string savePrice;
    public BigInteger price;
    public int clickCnt
    {
        get
        {
            if(!isEquip || petNum % 2 == 0)
            {
                return 0;
            }

            return petNum * level;
        }
    }

    public float clickTime
    {
        get
        {
            if(!isEquip || petNum % 2 == 1)
            {
                return 0;
            }

            return petNum + level * 1.25f;
        }
    }

    public bool isLocked
    {
        get
        {
            return level < 1;
        }
    }

    public bool isEquip;

    public Pet(int petNum, string petName, int amount, int level, int percent, BigInteger price)
    {
        this.petNum = petNum;
        this.petName = petName;
        this.amount = amount;
        this.price = price;
        this.level = level;
        this.percent = percent;
    }
}
