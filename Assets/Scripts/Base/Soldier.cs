using System.Collections.Generic;

[System.Serializable]
public class Soldier
{
    public string soldierName;
    public int soldierNum;
    public int amount;
    public int level;
    public long mPs;
    public long price;
    public long upgradePrice;
    public int percent;


    public Soldier(string soldierName, int soldierNum, int amount, long mPs, long price, long upgradePrice, int percent)
    {
        this.soldierName = soldierName;
        this.soldierNum = soldierNum;
        this.amount = amount;
        this.mPs = mPs;
        this.price = price;
        this.upgradePrice = upgradePrice;
        this.percent = percent;
    }
}

