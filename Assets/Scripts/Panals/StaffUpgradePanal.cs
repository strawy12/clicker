using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class StaffUpgradePanal : UpgradePanalBase
{
    [SerializeField] private Text staffNameText = null;
    [SerializeField] private Text staffInfoText = null;
    [SerializeField] private Image staffImage = null;


    private Sprite staffSprite = null;
    private Staff staff = null;
    private int staffNum;

    public override void LateUpdate()
    {
        if (!isShow) return;
        base.LateUpdate();
        ChangeBtnSprite(GameManager.Inst.CurrentUser.money, staff.price, staff.PriceSum(10), staff.PriceSum(100), true);;
    }

    public override void SetPanalNum(int num)
    {
        staff = GameManager.Inst.CurrentUser.staffs[num];
        staffSprite = GameManager.Inst.UI.SoldierSpriteArray[num];
        staffNum = num;
    }

    public override void UpdateValues()
    {


        staffImage.sprite = staffSprite;

        if (!staff.isLocked)
        {
            if(staff.isSold)
            {
                ChangeBuyBtnInfo("강화");
            }
            else
            {
                ChangeBuyBtnInfo("구매");
            }
            staffNameText.text = string.Format("Lv.{0} {1}", staff.level, staff.staffName);
            ChangeBuyBtnPriceText("원",staff.price, staff.PriceSum(10), staff.PriceSum(100));
            staffInfoText.text = string.Format("+ {0} / s", GameManager.Inst.MoneyUnitConversion(staff.mPs));
            staffImage.color = Color.white;
            backgroundImage.color = Color.white;
            buyBtnImages[2].sprite = GameManager.Inst.CurrentUser.money >= staff.price ? GameManager.Inst.UI.BuyBtnSpriteArray[isShow ? 3 : 1] : GameManager.Inst.UI.BuyBtnSpriteArray[isShow ? 2 : 0];
            bulkPurchaseBtn.gameObject.SetActive(!isShow);
        }
        else
        {
            staffNameText.text = "????";
            ChangeBuyBtnPriceText("");
            ChangeBuyBtnInfo("");
            staffInfoText.text = string.Format("조건: {0}의 Lv. 10 이상 달성", GameManager.Inst.CurrentUser.staffs[staffNum - 1].staffName);
            staffImage.color = Color.black;
            backgroundImage.color = Color.gray;
            buyBtnImages[2].sprite = GameManager.Inst.UI.BuyBtnSpriteArray[4];
            bulkPurchaseBtn.gameObject.SetActive(false);
        }
        
    }
    public void OnClickStaffBuyBtn(int amount)
    {
        UpgradeStaff(amount);
        if (amount != 1)
        {
            timer = 0f;
        }
    }

    private bool UpgradeStaff(int amount)
    {
        if (staff.isLocked) return false;
        if (GameManager.Inst.CurrentUser.money >= staff.price * amount)
        {
            GameManager.Inst.CurrentUser.UpdateMoney(staff.price * amount, false);
            staff.level += amount;
            GameManager.Inst.CurrentUser.levelUpCnt++;
            for (int i = 0; i < amount; i++)
            {
                staff.price = (BigInteger)((float)staff.price * 1.25f);
                staff.mPs = (BigInteger)Mathf.Max((float)staff.mPs * 1.25f, 10);
            }
           
            UpdateValues();
            GameManager.Inst.UI.UpdateMoneyPanal();
            GameManager.Inst.UI.ShowMessage("구매 완료");
            return true;
        }
        else
        {
            GameManager.Inst.UI.ShowMessage("돈이 부족합니다");
            return false;
        }
    }
    public override void ShowBulkPurchaseBtn()
    {
        base.ShowBulkPurchaseBtn();
    }

    public override void ReloadBulkPurchaseBtn()
    {
        base.ReloadBulkPurchaseBtn();
        ChangeBtnSprite(GameManager.Inst.CurrentUser.money, staff.price, staff.PriceSum(10), staff.PriceSum(100), false);
    }
}
