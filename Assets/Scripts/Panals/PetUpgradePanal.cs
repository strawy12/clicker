using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PetUpgradePanal : UpgradePanalBase
{
    [SerializeField] private Text petNameText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private Text amountText = null;
    [SerializeField] private Image petImage = null;
    [SerializeField] GameObject petObjectTemp = null;

    private Pet pet = null;
    private int petNum;
    private GameObject petBuffObj = null;

    public override void SetPanalNum(int num)
    {
        pet = GameManager.Inst.CurrentUser.pets[num];
        petNum = num;
        UpdateValues();
    }

    public override void UpdateValues()
    {
        petNameText.text = string.Format("Lv. {0} {1}", pet.level, pet.petName);
        priceText.text = string.Format("{0} ¿ø", pet.price);
        amountText.text = string.Format("{0}", pet.amount);
        //petImage.sprite = mainSprite;
    }

    public void OnClickMounting(bool isOn)
    {
        if(isOn)
        {
            if(petBuffObj == null)
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
