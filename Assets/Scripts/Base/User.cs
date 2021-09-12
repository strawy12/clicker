using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string userName;
    public long money;
    public long mileage;
    public long basemPc;
    public long mpc
    {
        get
        {
            return basemPc * Mathf.Max(1, (1 + peopleCnt) / 2);
        }
    }
    public long mPs;
    public int maxPeople;
    public int peopleCnt;
    public List<Soldier> soldiers = new List<Soldier>();
}
