using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UpgradePanal : MonoBehaviour
{
    [SerializeField] private Text soldierNameText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private Text countNumtText = null;
    [SerializeField] private Button contractBtn = null;
    [SerializeField] private Image soldierImage = null;
    [SerializeField] private EPanalState ePanalState;
    enum EPanalState {slave, company, level };

    int soldierNumber;
    [SerializeField] private Sprite[] soldierSpriteArray = null;


    private Soldier soldier = null;

    public void SetSoldierNum(int num)
    {
        soldierNumber = num;
        soldier = GameManager.Inst.CurrentUser.soldiers[soldierNumber];
        UpdateValues();
    }

    public void UpdateValues()
    {
        switch(ePanalState)
        {
            case EPanalState.company:
                soldierNameText.text = soldier.soldierName;
                priceText.text = string.Format("{0} 에너지", soldier.upgradePrice);
                countNumtText.text = string.Format("{0}", soldier.level);
                soldierImage.sprite = soldierSpriteArray[soldierNumber];
                break;
            case EPanalState.level:
                soldierNameText.text = soldier.soldierName;
                priceText.text = string.Format("{0} 에너지", soldier.upgradePrice);
                countNumtText.text = string.Format("{0}", soldier.level);
                soldierImage.sprite = soldierSpriteArray[soldierNumber];
                break;
            case EPanalState.slave:
                soldierNameText.text = soldier.soldierName;
                priceText.text = string.Format("{0} 에너지", soldier.price);
                countNumtText.text = string.Format("{0}", soldier.amount);
                soldierImage.sprite = soldierSpriteArray[soldierNumber];
                break;
        }
    }

    public void OnclickBtn()
    {
        switch(ePanalState)
        {
            case EPanalState.level:
                OnClickUpgradeBtn();
                break;
            case EPanalState.company:
                break;
            case EPanalState.slave:
                OnClickContractBtn();
                break;
        }
    }

    public void OnClickContractBtn()
    {
        if (GameManager.Inst.CurrentUser.maxPeople <= GameManager.Inst.CurrentUser.peopleCnt) return;
        if (GameManager.Inst.CurrentUser.money >= soldier.price)
        {
            GameManager.Inst.CurrentUser.money -= soldier.price;
            GameManager.Inst.CurrentUser.peopleCnt++;
            soldier.amount++;
            UpdateValues();
            GameManager.Inst.uiManager.SpawnJJikJJikE(soldierImage.sprite);
            GameManager.Inst.uiManager.UpdateEnergyPanal();
            StartCoroutine(GameManager.Inst.uiManager.Message("구매 완료"));
        }
        else
        {
            StartCoroutine(GameManager.Inst.uiManager.Message("돈이 부족합니다"));
        }
    }

    public void OnClickUpgradeBtn()
    {
        if (GameManager.Inst.CurrentUser.money >= soldier.price)
        {
            GameManager.Inst.CurrentUser.money -= soldier.price;
            soldier.level++;
            GameManager.Inst.CurrentUser.mPc += soldier.level * GameManager.Inst.CurrentUser.mPc / 3;
            soldier.upgradePrice = (long)(soldier.upgradePrice * 1.25f);
            UpdateValues();
            GameManager.Inst.uiManager.UpdateEnergyPanal();
            StartCoroutine(GameManager.Inst.uiManager.Message("구매 완료"));
        }
        else
        {
            StartCoroutine(GameManager.Inst.uiManager.Message("돈이 부족합니다"));
        }
    }

}