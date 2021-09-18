using System.Collections.Generic;

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

