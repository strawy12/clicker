using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSahyangPanal : UpgradePanalBase
{
    Sahayang sahayang;
    [SerializeField]Text nameText = null;
    [SerializeField] Text infoText = null;
    public override void Awake()
    {
        base.Awake();
    }

    public void SetPanalSahayng()
    {
        sahayang = GameManager.Inst.CurrentUser.sahayang;
    }

    public override void UpdateValues()
    {
        nameText.text = string.Format("Lv.{0} {1}", sahayang.level, "�ݻ���");
        infoText.text = string.Format("Ŭ�� �� {0}�� ȹ��", GameManager.Inst.MoneyUnitConversion(GameManager.Inst.CurrentUser.mPc));
        ChangeBuyBtnPriceText("��", sahayang.price, sahayang.PriceSum(10), sahayang.PriceSum(100));
        ChangeBuyBtnInfo("����");
        buyBtnImages[2].sprite = GameManager.Inst.CurrentUser.money >= sahayang.price ? GameManager.Inst.UI.BuyBtnSpriteArray[1] : GameManager.Inst.UI.BuyBtnSpriteArray[0];
    }
    public void UpgradeSahyang(int amount)
    {
        if (GameManager.Inst.CurrentUser.money >= sahayang.PriceSum(amount))
        {
            GameManager.Inst.CurrentUser.money -= sahayang.PriceSum(amount);
            GameManager.Inst.CurrentUser.levelUpCnt++;
            sahayang.level += amount;
            for (int i = 0; i < amount; i++)
            {
                sahayang.price = GameManager.Inst.MultiflyBigInteger(sahayang.price, 1.25f, 2);
            }
            UpdateValues();
            GameManager.Inst.UI.UpdateMoneyPanal();
            GameManager.Inst.UI.ShowMessage("���� �Ϸ�");
            SoundManager.Inst.SetEffectSound(3);
            return;
        }
        else
        {
            GameManager.Inst.UI.ShowMessage("���� �����մϴ�");
            SoundManager.Inst.SetEffectSound(1);
            return;
        }
    }


}
