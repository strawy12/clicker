using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

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

    private string spritePath = "Assets/Images/Clicker Button UI.png";

    public virtual void Awake()
    {
        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>(spritePath);
        spriteHandle.Completed += LoadSpriteWhenReady;
        backgroundImage = GetComponent<Image>();
        buyBtnImages = upgradeBtns.transform.GetComponentsInChildren<Image>();
        buyBtnInfoText = buyBtnImages[2].transform.GetChild(0).GetComponent<Text>();
        priceText = buyBtnImages[2].transform.GetChild(1).GetComponent<Text>();
        for (int i = 0; i < 2; i++)
        {
            buyBtnImages[i].gameObject.SetActive(false);
        }
    }
    protected virtual void LoadSpriteWhenReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            buyBtnSprites = handleToCheck.Result;
        }
        UpdateValues();
    }

    public T[] FindImages<T>(GameObject gameObject)
    {
        T[] arr = gameObject.GetComponentsInChildren<T>();
        T[] returnArr = new T[arr.Length - 1];
        for (int i = 1; i < arr.Length; i++)
        {
            returnArr[i - 1] = arr[i];
        }

        return returnArr;
    }

    public virtual void SetPanalNum(int num)
    {

    }

    public virtual void UpdateValues()
    {
        if (buyBtnSprites == null)
        {
            return;
        }
    }



    public void OnClickBulkPurchaseBtn()
    {
        isShow = true;

        buyBtnImages[2].sprite = buyBtnSprites[3];
        bulkPurchaseBtn.gameObject.SetActive(false);
        for (int i = 0; i < buyBtnImages.Length; i++)
        {
            buyBtnImages[i].gameObject.SetActive(true);
        }
        buyBtnImages[1].sprite = buyBtnSprites[2];
        buyBtnImages[0].sprite = buyBtnSprites[0];
        buyBtnImages[1].rectTransform.DOAnchorPosX(40f, 0.3f).OnComplete(() =>
        {
            buyBtnImages[0].rectTransform.DOAnchorPosX(-10f, 0.25f).OnComplete(() => Invoke("ReloadBulkPurchaseBtn", 3f));
        });
        
    }
    
    public void ReloadBulkPurchaseBtn()
    {
        isShow = false;
        for (int i = 0; i < 2; i++)
        {
            buyBtnImages[i].rectTransform.DOAnchorPosX(95f, 0f);
            buyBtnImages[i].gameObject.SetActive(false);
        }
        buyBtnImages[2].sprite = buyBtnSprites[1];
        bulkPurchaseBtn.gameObject.SetActive(true);

    }    
}