using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BigInteger = System.Numerics.BigInteger;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEngine.AddressableAssets;

public class UpgradePanalBase : MonoBehaviour
{
    [SerializeField] protected GameObject upgradeBtns = null;
    [SerializeField] protected Button bulkPurchaseBtn = null;
    protected Image backgroundImage = null;

    protected Image[] buyBtnImages = null;
    protected Text[] priceText = null;
    protected Text[] buyBtnInfoText = null;
    protected bool isLocked = true;
    protected bool isShow = false;
    protected float timer = 0f;


    public virtual void Awake()
    {
        //AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>(spritePath);
        //spriteHandle.Completed += LoadSpriteWhenReady;
        backgroundImage = GetComponent<Image>();
        buyBtnImages = upgradeBtns.transform.GetComponentsInChildren<Image>();
        buyBtnInfoText = new Text[buyBtnImages.Length];
        priceText = new Text[buyBtnImages.Length];
        for (int i = 0; i < buyBtnImages.Length; i++)
        {
            buyBtnInfoText[i] = buyBtnImages[i].transform.GetChild(0).GetComponent<Text>();
            priceText[i] = buyBtnImages[i].transform.GetChild(1).GetComponent<Text>();

        }
        for (int i = 0; i < 2; i++)
        {
            buyBtnImages[i].gameObject.SetActive(false);
        }
    }

    //protected virtual void LoadSpriteWhenReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    //{
    //    if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        buyBtnSprites = handleToCheck.Result;
    //    }
    //    
    //}


    public virtual void LateUpdate()
    {
        if (!isShow) return;
        timer += Time.deltaTime;
        if (timer >= 3f)
        {
            ReloadBulkPurchaseBtn();
        }
    }

    public void OnDisable()
    {
        if (isShow)
        {
            ReloadBulkPurchaseBtn();
        }
    }
    public virtual void SetPanalNum(int num)
    {

    }

    public virtual void UpdateValues()
    {
    }


    public virtual void ShowBulkPurchaseBtn()
    {
        isShow = true;

        bulkPurchaseBtn.gameObject.SetActive(false);
        for (int i = 0; i < buyBtnImages.Length; i++)
        {
            buyBtnImages[i].gameObject.SetActive(true);
        }

        buyBtnImages[1].rectTransform.DOAnchorPosX(25f, 0.3f).OnComplete(() =>
        {
            buyBtnImages[0].rectTransform.DOAnchorPosX(-45f, 0.25f);
        });
    }

    protected void ChangeBuyBtnInfo(string infoText)
    {
        for (int i = 0; i < buyBtnInfoText.Length; i++)
        {
            buyBtnInfoText[i].text = infoText;
        }
    }
    protected void ChangeBuyBtnPriceText(string text, BigInteger priceText, BigInteger priceText_10, BigInteger priceText_100)
    {

        this.priceText[0].text = string.Format("{0} {1}", GameManager.Inst.MoneyUnitConversion(priceText_100), text);
        this.priceText[1].text = string.Format("{0} {1}", GameManager.Inst.MoneyUnitConversion(priceText_10), text);
        this.priceText[2].text = string.Format("{0} {1}", GameManager.Inst.MoneyUnitConversion(priceText), text);

    }
    protected void ChangeBuyBtnPriceText(string text)
    {

        for (int i = 0; i < priceText.Length; i++)
        {
            priceText[i].text = text;
        }

    }

    protected void ChangeBtnSprite(BigInteger money, BigInteger price, BigInteger price_10, BigInteger price_100, bool isShow)
    {
        if (!this.isShow) return;

        if (isShow)
        {
            buyBtnImages[2].sprite = money >= price ? GameManager.Inst.UI.BuyBtnSpriteArray[3] : GameManager.Inst.UI.BuyBtnSpriteArray[2];
            buyBtnImages[1].sprite = money >= price_10 ? GameManager.Inst.UI.BuyBtnSpriteArray[3] : GameManager.Inst.UI.BuyBtnSpriteArray[2];
            buyBtnImages[0].sprite = money >= price_100 ? GameManager.Inst.UI.BuyBtnSpriteArray[1] : GameManager.Inst.UI.BuyBtnSpriteArray[0];
        }
        else
        {
            buyBtnImages[2].sprite = money >= price ? GameManager.Inst.UI.BuyBtnSpriteArray[1] : GameManager.Inst.UI.BuyBtnSpriteArray[0];
        }

    }

    public virtual void ReloadBulkPurchaseBtn()
    {
        isShow = false;
        timer = 0f;
        for (int i = 0; i < 2; i++)
        {
            buyBtnImages[i].rectTransform.DOAnchorPosX(95f, 0f);
            buyBtnImages[i].gameObject.SetActive(false);
        }

        bulkPurchaseBtn.gameObject.SetActive(true);

    }
}