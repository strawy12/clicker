using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPanalState = GameManager.EPanalState;
public class UpgradePanal : MonoBehaviour
{
    [SerializeField] private Text staffNameText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private Text countNumtText = null;
    [SerializeField] private Button contractBtn = null;
    [SerializeField] private Image staffImage = null;
    [SerializeField] private EPanalState ePanalState;

    private Sprite staffSprite = null;

    int staffNumber;


    private Staff staff = null;

    public void SetSoldierNum(int num)
    {
        staffNumber = num;
        staff = GameManager.Inst.CurrentUser.staffs[num];
        staffSprite = GameManager.Inst.UI.SoldierSpriteArray[num];
        UpdateValues();
        SpawnStaffObj();
    }

    public void UpdateValues()
    {
        switch(ePanalState)
        {
            case EPanalState.company:
                staffNameText.text = staff.staffName;
                priceText.text = string.Format("{0} 찍", staff.upgradePrice);
                countNumtText.text = string.Format("{0}", staff.level);
                staffImage.sprite = staffSprite;
                break;
            case EPanalState.level:
                staffNameText.text = staff.staffName;
                priceText.text = string.Format("{0} 찍", staff.upgradePrice);
                countNumtText.text = string.Format("{0}", staff.level);
                staffImage.sprite = staffSprite;
                break;
            case EPanalState.slave:
                staffNameText.text = staff.staffName;
                priceText.text = string.Format("{0} 찍", staff.price);
                countNumtText.text = string.Format("{0}", staff.amount);
                staffImage.sprite = staffSprite;
                break;
        }
    }

    private void SpawnStaffObj()
    {
        if (ePanalState != EPanalState.slave) return;
        if (staff.amount != 0)
        {
            for (int i = 0; i < staff.amount; i++)
            {
                GameManager.Inst.UI.SpawnStaff(staffImage.sprite, staffNumber);
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
            GameManager.Inst.UI.ShowMessage("보유 직원이 너무 많습니다");
            return;
        }
        if (GameManager.Inst.CurrentUser.mileage >= staff.price)
        {
            GameManager.Inst.CurrentUser.mileage -= staff.price;
            GameManager.Inst.CurrentUser.peopleCnt++;
            staff.amount++;
            UpdateValues();
            if(staff.amount == 1)
            {
                GameManager.Inst.UI.ActiveCompanySystemPanal(staffNumber, true);
            }
            GameManager.Inst.UI.SpawnStaff(staffImage.sprite, staffNumber);
            GameManager.Inst.UI.UpdateMoneyPanal();
            GameManager.Inst.UI.ShowMessage("구매 완료");
        }
        else
        {
            GameManager.Inst.UI.ShowMessage("돈이 부족합니다");
        }
    }

    public void OnClickUpgradeBtn()
    {
        if (GameManager.Inst.CurrentUser.money >= staff.price)
        {
            GameManager.Inst.CurrentUser.money -= staff.price;
            staff.level++;
            GameManager.Inst.CurrentUser.basezPc += staff.level * GameManager.Inst.CurrentUser.basezPc / 3;
            staff.upgradePrice = (long)(staff.upgradePrice * 1.25f);
            UpdateValues();
            GameManager.Inst.UI.UpdateMoneyPanal();
            GameManager.Inst.UI.ShowMessage("구매 완료");
        }
        else
        {
            GameManager.Inst.UI.ShowMessage("돈이 부족합니다");
        }
    }
}