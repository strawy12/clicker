using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaffUpgradePanal : UpgradePanalBase
{
    [SerializeField] private Text staffNameText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private Text levelText = null;
    [SerializeField] private Image staffImage = null;

    private Sprite staffSprite = null;
    private Staff staff = null;
    private int staffNum;

    public override void SetPanalNum(int num)
    {
        staff = GameManager.Inst.CurrentUser.staffs[num];
        staffSprite = GameManager.Inst.UI.SoldierSpriteArray[num];
        staffNum = num;
        UpdateValues();
    }

    public override void UpdateValues()
    {
        staffNameText.text = staff.staffName;
        priceText.text = string.Format("{0} 원", staff.price);
        levelText.text = string.Format("Lv. {0}", staff.level);
        staffImage.sprite = staffSprite;
    }
    public void OnClickStaffBuyBtn()
    {
        if (GameManager.Inst.CurrentUser.maxPeople <= GameManager.Inst.CurrentUser.peopleCnt)
        {
            GameManager.Inst.UI.ShowMessage("보유 직원이 너무 많습니다");
            return;
        }
        if (GameManager.Inst.CurrentUser.money >= staff.price)
        {
            GameManager.Inst.CurrentUser.money -= staff.price;
            GameManager.Inst.CurrentUser.peopleCnt++;
            staff.amount++;
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
