using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;

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


    public override void SetPanalNum(int num)
    {
        pet = GameManager.Inst.CurrentUser.pets[num];
        petNum = num;
        OnClickMounting(pet.isEquip);
    }

    public override void UpdateValues()
    {
        base.UpdateValues();

        if (!pet.isLocked)
        {
            petMountingBtn.isOn = pet.isEquip;
            petMountingBtn.interactable = true;
            buyBtnInfoText.text = "°­È­";
            petNameText.text = string.Format("Lv.{0} {1}", pet.level, pet.petName);
            priceText.text = string.Format("{0} / {1}\n{2} ¿ø",pet.amount, pet.maxAmount , pet.price);
            backgroundImage.color = Color.white;
            amountText.text = string.Format("{0}", pet.amount);
            bulkPurchaseBtn.gameObject.SetActive(!isShow);
            buyBtnImages[2].sprite = GameManager.Inst.CurrentUser.money >= pet.price && pet.amount >= pet.maxAmount ? buyBtnSprites[isShow ? 3 : 1] : buyBtnSprites[0];
            
        }
        else
        {
            petMountingBtn.interactable = false;
            petNameText.text = "????";
            priceText.text = "";
            buyBtnInfoText.text = "";
            backgroundImage.color = Color.gray;
            buyBtnImages[2].sprite = buyBtnSprites[4];
            bulkPurchaseBtn.gameObject.SetActive(false);
        }
        //petImage.sprite = mainSprite;
    }

    public void OnClickLevelUpBtn()
    {
        if (pet.level >= 10 || pet.amount < pet.maxAmount) return;
        if (GameManager.Inst.CurrentUser.money < pet.price) return;
        GameManager.Inst.CurrentUser.money -= pet.price;
        pet.amount -= pet.maxAmount;
        pet.level++;
        pet.price = (long)(pet.price * 1.25f);
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
