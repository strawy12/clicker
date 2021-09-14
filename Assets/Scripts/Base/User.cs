using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string userName;
    public long money;
    public long mileage;
    public long basezPc;

    public long zpc
    {
        get
        {
            return basezPc * (1 + peopleCnt);
        }
    }
    public long mPs;
    public int maxPeople;
    public int peopleCnt;
    public List<Staff> staffs = new List<Staff>();
}
