using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Staff
{
    public string staffName;
    public int staffNum;
    public int amount;
    public int level;
    public long mPs;
    public long price;
    public long upgradePrice;
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

    public Staff(string staffName, int staffNum, int amount, long mPs, long price, long upgradePrice)
    {
        this.staffName = staffName;
        this.staffNum = staffNum;
        this.amount = amount;
        this.mPs = mPs;
        this.price = price;
        this.upgradePrice = upgradePrice;
    }
}

