using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using System;

[Serializable]
public class User
{
    public string userName;
    public float playTime;
    public string exitTime;
    public string saveMoney;
    public long goldCoin;
    public bool[] missions
    {
        get
        {
            bool[] _mission = new bool[5];
            _mission[0] = clickCnt >= 2000;
            _mission[1] = bigHeartClickCnt >= 30;
            _mission[2] = skillUseCnt >= 5;
            _mission[3] = playTime >= 1800f;
            _mission[4] = levelUpCnt >= 5;
            return _mission;
        }
    }
    public int missionClear
    {
        get
        {
            int cnt = 0;
            for (int i = 0; i < missions.Length; i++)
            {
                if (missions[i])
                {
                    cnt++;
                }
            }
            return cnt;
        }
    }

    public BigInteger money;


    public int clickCnt;
    public int bigHeartClickCnt;
    public int skillUseCnt;
    public int levelUpCnt;
    public int additionMoney;
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
                if (pet.isEquip && pet.amount != 0)
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
        if (isSave)
        {
            saveMoney = money.ToString();
            saveBasemPc = basemPc.ToString();
            foreach (Staff staff in staffs)
            {
                staff.savemPs = staff.mPs.ToString();
                staff.savePrice = staff.price.ToString();
            }
            foreach (Pet pet in pets)
            {
                pet.savePrice = pet.price.ToString();
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
        }
    }

    public void UpdateMoney(BigInteger updateMoney, bool isAdd)
    {
        if(isAdd)
        {
            money += updateMoney;
        }
        else
        {
            money -= updateMoney;
        }
        GameManager.Inst.UI.UpdateMoneyPanal();
    }
}
