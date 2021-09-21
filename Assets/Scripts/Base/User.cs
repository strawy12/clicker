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

    public string MoneyUnitConversion(BigInteger value)
    {
        List<int> moneyList = new List<int>();
        int place = (int)Mathf.Pow(10, 4);
        string retStr = "";
        string unit = "";
        do
        {
            moneyList.Add((int)(value % place));
            value /= place;
        } while (value > 0);

        if (moneyList.Count > GameManager.Inst.moneyUnit.Length)
        {
            int cnt = (moneyList.Count - 1) / (GameManager.Inst.moneyUnit.Length - 1);
            int newCnt = (int)Mathf.Log10(cnt) + 1;
            string newUnit;
            unit = GameManager.Inst.moneyUnit[newCnt].ToString();
            newUnit = GameManager.Inst.moneyUnit[cnt].ToString();
            unit = unit + newUnit;
        }

        else
        {
            unit = GameManager.Inst.moneyUnit[moneyList.Count - 1].ToString();
        }
        if (moneyList.Count - 1 > 0)
        {

            retStr = string.Format("{0}{1}.{2}", moneyList[moneyList.Count - 1], unit, moneyList[moneyList.Count - 2]);
        }
        else
        {
            retStr = string.Format("{0}{1}", moneyList[moneyList.Count - 1], unit);
        }

        

        return retStr;
    }

    public void ConversionType(bool isSave)
    {
        if(isSave)
        {
            saveMoney = money.ToString();
            saveBasemPc = basemPc.ToString();
            foreach(Staff staff in staffs)
            {
                staff.savemPs = staff.mPs.ToString();
            }
        }
        else
        {

            money = BigInteger.Parse(saveMoney);
            basemPc = BigInteger.Parse(saveBasemPc);
            foreach (Staff staff in staffs)
            {
                staff.mPs = BigInteger.Parse(staff.savemPs);
            }
        }
    }
}
