using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class StaffUpgradePanal : UpgradePanalBase
{
    [SerializeField] private Text staffNameText = null;
    [SerializeField] private Image staffImage = null;
    [SerializeField] private Image backgroundImage = null;

    private Text priceText = null;
    private Text buyBtnInfoText = null;
    private Sprite staffSprite = null;
    private Staff staff = null;
    private int staffNum;

    public override void Awake()
    {
        base.Awake();
        buyBtnInfoText = buyBtn.transform.GetChild(0).GetComponent<Text>();
        priceText = buyBtn.transform.GetChild(1).GetComponent<Text>();
    }

    protected override void LoadSpriteWhenReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        base.LoadSpriteWhenReady(handleToCheck);
        UpdateValues();
    }

    public override void SetPanalNum(int num)
    {
        staff = GameManager.Inst.CurrentUser.staffs[num];
        staffSprite = GameManager.Inst.UI.SoldierSpriteArray[num];
        staffNum = num;
    }

    public override void UpdateValues()
    {
        if (buyBtnSprites == null)
        {
            return;
        }

        staffImage.sprite = staffSprite;

        if (!staff.isLocked)
        {
            staffNameText.text = string.Format("Lv.{0} {1}", staff.staffName, staff.level);
            priceText.text = string.Format("{0} 원", staff.price);
            buyBtnInfoText.text = "구매";
            staffImage.color = Color.white;
            backgroundImage.color = Color.white;
            buyBtnImage.sprite = GameManager.Inst.CurrentUser.money >= staff.price ? buyBtnSprites[1] : buyBtnSprites[0];
            bulkPurchaseBtn.gameObject.SetActive(true);
        }
        else
        {
            staffNameText.text = "????";
            priceText.text = "";
            buyBtnInfoText.text = "";
            staffImage.color = Color.black;
            backgroundImage.color = Color.gray;
            buyBtnImage.sprite = buyBtnSprites[4];
            bulkPurchaseBtn.gameObject.SetActive(false);
        }
        
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
