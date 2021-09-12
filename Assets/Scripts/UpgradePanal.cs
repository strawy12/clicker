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

    private Sprite soldierSprite = null;
    enum EPanalState {slave, company, level };

    int soldierNumber;


    private Soldier soldier = null;

    public void SetSoldierNum(int num)
    {
        soldierNumber = num;
        soldier = GameManager.Inst.CurrentUser.soldiers[num];
        soldierSprite = GameManager.Inst.uiManager.SoldierSpriteArray[num];
        UpdateValues();
        SpawnSlaveObj();
    }

    public void UpdateValues()
    {
        switch(ePanalState)
        {
            case EPanalState.company:
                soldierNameText.text = soldier.soldierName;
                priceText.text = string.Format("{0} ������", soldier.upgradePrice);
                countNumtText.text = string.Format("{0}", soldier.level);
                soldierImage.sprite = soldierSprite;
                break;
            case EPanalState.level:
                soldierNameText.text = soldier.soldierName;
                priceText.text = string.Format("{0} ������", soldier.upgradePrice);
                countNumtText.text = string.Format("{0}", soldier.level);
                soldierImage.sprite = soldierSprite;
                break;
            case EPanalState.slave:
                soldierNameText.text = soldier.soldierName;
                priceText.text = string.Format("{0} ���ϸ���", soldier.price);
                countNumtText.text = string.Format("{0}", soldier.amount);
                soldierImage.sprite = soldierSprite;
                break;
        }
    }

    private void SpawnSlaveObj()
    {
        if (ePanalState != EPanalState.slave) return;
        if (soldier.amount != 0)
        {
            for (int i = 0; i < soldier.amount; i++)
            {
                GameManager.Inst.uiManager.SpawnJJikJJikE(soldierImage.sprite, soldierNumber);
            }
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
        if (GameManager.Inst.CurrentUser.maxPeople <= GameManager.Inst.CurrentUser.peopleCnt)
        {
            GameManager.Inst.uiManager.ShowMessage("���� �뿹�� �ʹ� �����ϴ�");
            return;
        }
        if (GameManager.Inst.CurrentUser.mileage >= soldier.price)
        {
            GameManager.Inst.CurrentUser.mileage -= soldier.price;
            GameManager.Inst.CurrentUser.peopleCnt++;
            soldier.amount++;
            UpdateValues();
            if(soldier.amount == 1)
            {
                GameManager.Inst.uiManager.ActiveCompanySystemPanal(soldierNumber, true);
            }
            GameManager.Inst.uiManager.SpawnJJikJJikE(soldierImage.sprite, soldierNumber);
            GameManager.Inst.uiManager.UpdateEnergyPanal();
            GameManager.Inst.uiManager.ShowMessage("���� �Ϸ�");
        }
        else
        {
            GameManager.Inst.uiManager.ShowMessage("���� �����մϴ�");
        }
    }

    public void OnClickUpgradeBtn()
    {
        if (GameManager.Inst.CurrentUser.money >= soldier.price)
        {
            GameManager.Inst.CurrentUser.money -= soldier.price;
            soldier.level++;
            GameManager.Inst.CurrentUser.basemPc += soldier.level * GameManager.Inst.CurrentUser.basemPc / 3;
            soldier.upgradePrice = (long)(soldier.upgradePrice * 1.25f);
            UpdateValues();
            GameManager.Inst.uiManager.UpdateEnergyPanal();
            GameManager.Inst.uiManager.ShowMessage("���� �Ϸ�");
        }
        else
        {
            GameManager.Inst.uiManager.ShowMessage("���� �����մϴ�");
        }
    }
}