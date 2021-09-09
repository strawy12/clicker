using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanal : MonoBehaviour
{
    [SerializeField] private Text soldierNameText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private Text amountText = null;
    [SerializeField] private Button purchaseBtton = null;

    private Soldier soldier = null;

    public void SetValue(Soldier soldier)
    {
        this.soldier = soldier;
        UpdateUpgradePanalUI();
    }

    public void UpdateUpgradePanalUI()
    {
        soldierNameText.text = soldier.soldierName;
        priceText.text = string.Format("{0} 에너지", soldier.price);
        amountText.text = string.Format("{0}", soldier.amount);
    }

    public void OnClickPurchase()
    {
        if(GameManager.Inst.CurrentUser.energy >= soldier.price)
        {
            GameManager.Inst.CurrentUser.energy -= soldier.price;
            soldier.amount++;
            soldier.price = (long)(soldier.price * 1.25f);
            UpdateUpgradePanalUI();
            GameManager.Inst.UI.UpdateEnergyPanal();
            StartCoroutine(GameManager.Inst.UI.Message("구매 완료"));
        }
        else
        {
            StartCoroutine(GameManager.Inst.UI.Message("돈이 부족합니다"));
        }
    }
}
