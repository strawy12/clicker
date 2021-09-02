using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanal : MonoBehaviour
{
    [SerializeField] private Text soldierNameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text amountText;
    [SerializeField] private Button purchaseBtton;

    private Soldier soldier;

    public void SetValue(Soldier soldier)
    {
        soldierNameText.text = soldier.soldierName;
        priceText.text = string.Format("{0} ¿¡³ÊÁö", soldier.price);
        amountText.text = string.Format("{0}", soldier.amount);
    }
}
