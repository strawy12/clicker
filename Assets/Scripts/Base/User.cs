using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using System;

[Serializable]
public class User
{
    public string userName;
    public string exitTime;
    public string saveMoney;

    public BigInteger money;

    public int additionMoney;
    public long mileage;
    public string saveBasemPc;
    public BigInteger basemPc;
    public BigInteger mPc
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

    //public void AddMoney(int money)

    public void ConversionType(bool isSave)
    {
        if(isSave)
        {
            saveMoney = money.ToString();
            saveBasemPc = basemPc.ToString();
            foreach(Staff staff in staffs)
            {
                staff.savemPs = staff.mPs.ToString();
                staff.savePrice = staff.price.ToString();
            }
            foreach (Pet pet in pets)
            {
                pet.savePrice = pet.price.ToString();
            }
            foreach (Skill skill in skills)
            {
                skill.savePrice = skill.price.ToString();
            }
        }
        else
        {

            money = BigInteger.Parse(saveMoney);
            basemPc = BigInteger.Parse(saveBasemPc);
            foreach (Staff staff in staffs)
            {
                staff.mPs = BigInteger.Parse(staff.savemPs);
                staff.price = BigInteger.Parse(staff.savePrice);
            }
            foreach (Pet pet in pets)
            {
                pet.price = BigInteger.Parse(pet.savePrice);
            }
            foreach (Skill skill in skills)
            {
                skill.price = BigInteger.Parse(skill.savePrice);
            }
        }
    }
}
