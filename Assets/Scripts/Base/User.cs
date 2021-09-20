using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string userName;
    public long money;
    public long additionMoney;
    public long mileage;
    public long basemPc;
    public long mPc
    {
        get
        {
            return basemPc * Mathf.Max(1, (petAmount * 2)) * additionMoney;
        }
    }

    public int petAmount
    {
        get
        {
            int cnt = 0;
            foreach (Pet pet in pets)
            {
                if (pet.isEquip && pet.amount != 0)
                {
                    cnt += pet.clickCnt;
                }
            }
            return cnt;
        }
    }

    public float autoClickTime
    {
        get
        {
            float time = 120f;
            foreach (Pet pet in pets)
            {
                if(pet.isEquip && pet.amount != 0)
                {
                    time -= pet.clickTime;
                }
            }
            
            time = Mathf.Max(5f, time);
            return time;
        }
    }
    public List<Staff> staffs = new List<Staff>();
    public List<Skill> skills = new List<Skill>();
    public List<Pet> pets = new List<Pet>();

}
