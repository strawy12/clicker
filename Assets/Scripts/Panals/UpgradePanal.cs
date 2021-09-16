using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPanalState = GameManager.EPanalState;
public class UpgradePanal : MonoBehaviour
{
    [SerializeField] private Text nameText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private Text countNumtText = null;
    [SerializeField] private Button buyBtn = null;
    [SerializeField] private Image mainImage = null;
    [SerializeField] private EPanalState ePanalState;

    private Sprite mainSprite = null;

    int panal_Number;


    private Staff staff = null;
    private Skill skill = null;

    public void SetPanalNum(int num, EPanalState state)
    {
        switch(state)
        {
            case EPanalState.company:
                SetCompanyNum(num);
                break;
            case EPanalState.staff:
                SetStaffNum(num);
                break;
            case EPanalState.level:
                break;
        }
        
    }

    private void SetStaffNum(int num)
    {
        panal_Number = num;
        staff = GameManager.Inst.CurrentUser.staffs[num];
        mainSprite = GameManager.Inst.UI.SoldierSpriteArray[num];
        UpdateValues();
        SpawnStaffObj();
    }

    private void SetCompanyNum(int num)
    {
        panal_Number = num;
        skill = GameManager.Inst.CurrentUser.skills[num];
        //mainSprite = GameManager.Inst.UI.SoldierSpriteArray[num];
        UpdateValues();
        //SpawnStaffObj();
    }
    public void UpdateValues()
    {
        switch(ePanalState)
        {
            case EPanalState.company:
                nameText.text = skill.skillName;
                priceText.text = string.Format("{0} 원", skill.price);
                countNumtText.text = string.Format("{0}", skill.level);
                //mainImage.sprite = mainSprite;
                break;
            case EPanalState.level:
                nameText.text = staff.staffName;
                priceText.text = string.Format("{0} 원", staff.upgradePrice);
                countNumtText.text = string.Format("{0}", staff.level);
                mainImage.sprite = mainSprite;
                break;
            case EPanalState.staff:
                nameText.text = staff.staffName;
                priceText.text = string.Format("{0} 원", staff.price);
                countNumtText.text = string.Format("{0}", staff.amount);
                mainImage.sprite = mainSprite;
                break;
        }
    }

    private void SpawnStaffObj()
    {
        if (ePanalState != EPanalState.staff) return;
        if (staff.amount != 0)
        {
            for (int i = 0; i < staff.amount; i++)
            {
                GameManager.Inst.UI.SpawnStaff(mainImage.sprite, panal_Number);
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
            case EPanalState.staff:
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
                GameManager.Inst.UI.ActiveCompanySystemPanal(panal_Number, true);
            }
            GameManager.Inst.UI.SpawnStaff(mainImage.sprite, panal_Number);
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