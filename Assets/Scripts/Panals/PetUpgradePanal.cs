using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using BigInteger = System.Numerics.BigInteger;

public class PetUpgradePanal : UpgradePanalBase
    {
        [SerializeField] private Text petNameText = null;
        [SerializeField] private Text amountText = null;
        [SerializeField] private Image petImage = null;
        [SerializeField] GameObject petObjectTemp = null;
        [SerializeField] private Toggle petMountingBtn = null;

        private Pet pet = null;
        private int petNum;
    private GameObject petBuffObj = null;

    public override void Awake()
    {
        buyBtnSprites = Resources.LoadAll<Sprite>(spritePath);
        backgroundImage = GetComponent<Image>();
        buyBtnImages = upgradeBtns.transform.GetComponentsInChildren<Image>();
        buyBtnInfoText = buyBtnImages[0].transform.GetChild(0).GetComponent<Text>();
        priceText = buyBtnImages[0].transform.GetChild(1).GetComponent<Text>();
    }

    public override void SetPanalNum(int num)
    {
        pet = GameManager.Inst.CurrentUser.pets[num];
        petNum = num;
        OnClickMounting(pet.isEquip);
    }

    public override void UpdateValues()
    {

        if (!pet.isLocked)
        {
            petMountingBtn.isOn = pet.isEquip;
            petMountingBtn.interactable = true;
            buyBtnInfoText.text = "강화";
            petNameText.text = string.Format("Lv.{0} {1}", pet.level, pet.petName);
            priceText.text = string.Format("{0} / {1}\n{2} 원",pet.amount, pet.maxAmount , GameManager.Inst.MoneyUnitConversion(pet.price));
            backgroundImage.color = Color.white;
            amountText.text = string.Format("{0}", pet.amount);
            bulkPurchaseBtn.gameObject.SetActive(!isShow);
            buyBtnImages[0].sprite = GameManager.Inst.CurrentUser.money >= pet.price && pet.amount >= pet.maxAmount ? buyBtnSprites[isShow ? 3 : 1] : buyBtnSprites[0];
            
        }
        else
        {
            petMountingBtn.interactable = false;
            petNameText.text = "????";
            priceText.text = "";
            buyBtnInfoText.text = "";
            backgroundImage.color = Color.gray;
            buyBtnImages[0].sprite = buyBtnSprites[4];
        }
        //petImage.sprite = mainSprite;
    }

    public void OnClickLevelUpBtn()
    {
        if (pet.isLocked) return;
        if (pet.level >= 10 || pet.amount < pet.maxAmount)
        {

            GameManager.Inst.UI.ShowMessage(pet.level >= 10 ? "펫의 레벨이 최대레벨입니다." : "펫의 갯수가 부족합니다.");
            return;
        }
        if (GameManager.Inst.CurrentUser.money < pet.price)
        {
            GameManager.Inst.UI.ShowMessage("돈이 부족합니다.");
            return;
        }
        GameManager.Inst.CurrentUser.money -= pet.price;
        pet.amount -= pet.maxAmount;
        pet.level++;
        pet.price = (BigInteger)((float)pet.price * 1.25f);
        GameManager.Inst.UI.ShowMessage("레벨업!");
    }

    public void OnClickMounting(bool isOn)
    {
        pet.isEquip = isOn;
        if (petBuffObj == null)
        {
            petBuffObj = Instantiate(petObjectTemp, petObjectTemp.transform.parent);
        }
        if (isOn)
        {
            //buffPanal.transform.GetChild(0).GetComponent<Image>().sprite = petSprite;
            petBuffObj.SetActive(true);
            petBuffObj.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutBack);
        }

        else
        {
            petBuffObj.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InOutBack).OnComplete(() => petBuffObj.SetActive(false));
        }
    }
}
