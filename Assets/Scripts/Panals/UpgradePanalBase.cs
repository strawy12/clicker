using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class UpgradePanalBase : MonoBehaviour
{
    [SerializeField] protected Button buyBtn = null;
    [SerializeField] protected Button bulkPurchaseBtn = null;

    protected Sprite[] buyBtnSprites = null;
   
    protected Image[] buyBtnImages = null;
    protected Image buyBtnImage = null;

    protected bool isLocked = true;

    private string spritePath = "Assets/Images/Clicker Button UI.png";

    public virtual void Awake()
    {
        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>(spritePath);
        spriteHandle.Completed += LoadSpriteWhenReady;
        buyBtnImages = FindImages<Image>(bulkPurchaseBtn.gameObject);
        buyBtnImage = buyBtn.GetComponent<Image>();
    }
    protected virtual void LoadSpriteWhenReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            buyBtnSprites = handleToCheck.Result;
        }
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

    public virtual void SetPanalNum(int num)
    {

    }

    public virtual void UpdateValues()
    {

    }

    

    public void OnClickBulkPurchaseBtn()
    {
        buyBtnImage.sprite = buyBtnSprites[3];
        for(int i = 0; i < buyBtnImages.Length; i++)
        {
            buyBtnImages[i].sprite = buyBtnSprites[2];
            buyBtnImages[i].gameObject.SetActive(true);
        }
        buyBtnImages[1].GetComponent<RectTransform>().DOAnchorPosX(-5f, 0.3f).OnComplete( () =>
        {
            buyBtnImages[0].GetComponent<RectTransform>().DOAnchorPosX(-50f, 0.25f);
        });
    }

}