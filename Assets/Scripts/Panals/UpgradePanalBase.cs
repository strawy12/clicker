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

    protected Sprite[] buyBtnSprites = null;

    protected Image[] buyBtnImages = null;
    protected Text priceText = null;
    protected Text buyBtnInfoText = null;
    protected bool isLocked = true;
    protected bool isShow = false;
    protected float timer = 0f;

    protected string spritePath = "Clicker Button UI";

    public virtual void Awake()
    {
        //AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>(spritePath);
        //spriteHandle.Completed += LoadSpriteWhenReady;
        buyBtnSprites = Resources.LoadAll<Sprite>(spritePath);
        backgroundImage = GetComponent<Image>();
        buyBtnImages = upgradeBtns.transform.GetComponentsInChildren<Image>();
        buyBtnInfoText = buyBtnImages[2].transform.GetChild(0).GetComponent<Text>();
        priceText = buyBtnImages[2].transform.GetChild(1).GetComponent<Text>();
        for (int i = 0; i < 2; i++)
        {
            buyBtnImages[i].gameObject.SetActive(false);
        }
    }
    public void Start()
    {
        UpdateValues();
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
        if(isShow)
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

        buyBtnImages[1].rectTransform.DOAnchorPosX(40f, 0.3f).OnComplete(() =>
        {
            buyBtnImages[0].rectTransform.DOAnchorPosX(-10f, 0.25f);
        });
    }

    protected void ChangeBtnSprite(BigInteger price, bool isShow)
    {
        if (!this.isShow) return;
        Debug.Log("¾Ó¤·");
        if (isShow)
        {
            buyBtnImages[2].sprite = GameManager.Inst.CurrentUser.money >= price ? buyBtnSprites[3] : buyBtnSprites[2];
            buyBtnImages[1].sprite = GameManager.Inst.CurrentUser.money >= price * 10 ? buyBtnSprites[3] : buyBtnSprites[2];
            buyBtnImages[0].sprite = GameManager.Inst.CurrentUser.money >= price * 100 ? buyBtnSprites[1] : buyBtnSprites[0];
        }
        else
        {
            buyBtnImages[2].sprite = GameManager.Inst.CurrentUser.money >= price ? buyBtnSprites[1] : buyBtnSprites[0];
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