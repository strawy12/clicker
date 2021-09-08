using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UpgradePanal : MonoBehaviour
{
    [SerializeField] private Text soldierNameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text amountText;
    [SerializeField] private Button contractBtn;
    [SerializeField] private Image soldierImage;

    int soldierNumber;
    private Sprite[] soldierSpriteArray;

    private string spritePath = "Assets/Images/SahyangClickerSoldier.png";

    private Soldier soldier;

    private void Awake()
    {
        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>(spritePath);
        spriteHandle.Completed += LoadSpriteWhenReady;
    }

    private void LoadSpriteWhenReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            soldierSpriteArray = handleToCheck.Result;
            UpdateValues();
        }
    }

    public void SetSoldierNum(int num)
    {
        soldierNumber = num;
    }

    public void UpdateValues()
    {
        soldier = GameManager.Inst.CurrentUser.soldiers[soldierNumber];
        soldierNameText.text = soldier.soldierName;
        priceText.text = string.Format("{0} ¿¡³ÊÁö", soldier.price);
        amountText.text = string.Format("{0}", soldier.amount);
        soldierImage.sprite = soldierSpriteArray[soldierNumber];
    }

    public void OnClickContractBtn()
    {
        if (GameManager.Inst.CurrentUser.money >= soldier.price)
        {
            GameManager.Inst.CurrentUser.money -= soldier.price;
            GameManager.Inst.CurrentUser.soldiers[soldierNumber].amount++;
            UpdateValues();
            GameManager.Inst.uiManager.SpawnJJikJJikE(soldierImage.sprite);
        }
    }
}