using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EPanalState = GameManager.EPanalState;
public class UpgradePanal : MonoBehaviour
{
    [SerializeField] private Text nameText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private Text countNumtText = null;
    [SerializeField] private Image buyBtn = null;
    [SerializeField] private Button bulkPurchaseBtn = null;
    [SerializeField] private Image mainImage = null;
    [SerializeField] private EPanalState ePanalState;
    [SerializeField] private Sprite[] activeBuyBtnSprites = null;
    [SerializeField] private Sprite[] inActiveBuyBtnSprites = null;
    private Sprite mainSprite = null;
    private Image[] buyBtnImages = null; 
    int panal_Number;


    private Staff staff = null;
    private Skill skill = null;
    private void Awake()
    {
        buyBtnImages = FindImages<Image>(bulkPurchaseBtn.gameObject);
    }
    public T[] FindImages<T>(GameObject gameObject)
    {
        T[] arr = gameObject.GetComponentsInChildren<T>();
        T[] returnArr = new T[arr.Length - 1];
        for(int i = 1; i < arr.Length; i++)
        {
            returnArr[i -1] = arr[i];
        }

        return returnArr;
    }

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

    public void OnClickBulkPurchaseBtn()
    {
        buyBtn.sprite = activeBuyBtnSprites[1];
        for(int i = 0; i < buyBtnImages.Length; i++)
        {
            buyBtnImages[i].sprite = inActiveBuyBtnSprites[1];
            buyBtnImages[i].gameObject.SetActive(true);
        }
        buyBtnImages[1].GetComponent<RectTransform>().DOAnchorPosX(-5f, 0.3f).OnComplete( () =>
        {
            buyBtnImages[0].GetComponent<RectTransform>().DOAnchorPosX(-50f, 0.25f);
        });
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
                OnClickStaffBuyBtn();
                break;
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
            GameManager.Inst.CurrentUser.peopleCnt++;
            staff.amount++;
            UpdateValues();
            GameManager.Inst.UI.UpdateMoneyPanal();
            SpawnStaffObj();
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
            GameManager.Inst.CurrentUser.mPs += panal_Number * GameManager.Inst.CurrentUser.basemPc / 3;
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