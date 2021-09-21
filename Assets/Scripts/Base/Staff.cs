using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public class Staff
{
    public string staffName;
    public int staffNum;
    public int level;
    public string savemPs;
    public BigInteger mPs;
    public long price;
    public long upgradePrice;

    public bool isSold
    {
        get
        {
            return level > 0;
        }
    }

    public bool isLocked
    {
        get
        {
            int num = staffNum - 1;
            if (num < 0) 
            {
                return false;
            }

            return GameManager.Inst.CurrentUser.staffs[num].level < 10;
        }
    }

    public Staff(string staffName, int staffNum, int level, BigInteger mPs, long price, long upgradePrice)
    {
        this.staffName = staffName;
        this.staffNum = staffNum;
        this.level = level;
        this.mPs = mPs;
        this.price = price;
        this.upgradePrice = upgradePrice;
    }
}

