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
    protected override void LoadSpriteWhenReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        base.LoadSpriteWhenReady(handleToCheck);
        UpdateValues();
    }
    public override void SetPanalNum(int num)
    {
        pet = GameManager.Inst.CurrentUser.pets[num];
        petNum = num;
    }

    public override void UpdateValues()
    {
        base.UpdateValues();

        if (!pet.isLocked)
        {
            petMountingBtn.interactable = true;
            buyBtnInfoText.text = "°­È­";
            petNameText.text = string.Format("Lv.{0} {1}", pet.level, pet.petName);
            priceText.text = string.Format("{0} ¿ø", pet.price);
            backgroundImage.color = Color.white;
            buyBtnImages[2].sprite = GameManager.Inst.CurrentUser.money >= pet.price ? buyBtnSprites[isShow ? 3 : 1] : buyBtnSprites[0];
            bulkPurchaseBtn.gameObject.SetActive(!isShow);
            amountText.text = string.Format("{0}", pet.amount);
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

    public void OnClickMounting(bool isOn)
    {
        if (isOn)
        {
            if (petBuffObj == null)
            {
                petBuffObj = Instantiate(petObjectTemp, petObjectTemp.transform.parent);
            }

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
