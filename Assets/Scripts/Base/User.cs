using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string userName;
    public long money;
    public long mileage;
    public long basemPc;

    public long mPc
    {
        get
        {
            return basemPc * (1 + peopleCnt) * Mathf.Max(1, (petAmount * 2));
        }
    }
    public long mPs;
    public int maxPeople;
    public int peopleCnt
    {
        get
        {
            int cnt = 0;
            foreach(Staff staff in staffs)
            {
                if(staff.level != 0)
                {
                    cnt++;
                }
            }
            return cnt;
        }
    }

    private int petAmount
    {
        get
        {
            int cnt = 0;
            foreach (Pet pet in pets)
            {
                if (pet.amount != 0)
                {
                    cnt++;
                }
            }
            return cnt;
        }
    }
    public List<Staff> staffs = new List<Staff>();
    public List<Skill> skills = new List<Skill>();
    public List<Pet> pets = new List<Pet>();
}
