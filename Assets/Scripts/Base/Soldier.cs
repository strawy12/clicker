using System.Collections.Generic;

[System.Serializable]
public class Soldier
{
    public string soldierName;
    public int amount;
    public int level;
    public long mPs;
    public long price;
    public long upgradePrice;

    public Soldier(string soldierName, int amount, long mPs, long price, long upgradePrice)
    {
        this.soldierName = soldierName;
        this.amount = amount;
        this.mPs = mPs;
        this.price = price;
        this.upgradePrice = upgradePrice;
    }
}

