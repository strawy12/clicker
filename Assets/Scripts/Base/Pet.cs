using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pet
{
    public int petNum;
    public string petName;
    public int amount;
    public int level;
    public long price;

    public Pet(int petNum, string petName, int amount,int level, int price)
    {
        this.petNum = petNum;
        this.petName = petName;
        this.amount = amount;
        this.level = level;
        this.price = price;
    }
}
