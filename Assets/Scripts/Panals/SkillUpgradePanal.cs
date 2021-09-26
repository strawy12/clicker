using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BigInteger = System.Numerics.BigInteger;

public class SkillUpgradePanal : UpgradePanalBase
{
    [SerializeField] private Text skillNameText = null;
    [SerializeField] private Image skillImage = null;
    [SerializeField] private Image coolTime = null;
    private Skill skill = null;
    private int skillNum;
    private DateTime endTime;
    private DateTime endDurationTime;

    public override void LateUpdate()
    {
        if (skill.isUsed)
        {
            coolTime.fillAmount = (float)CheckCoolTime(endTime, DateTime.Now) / skill.coolTime;
            if (DateTime.Now > endDurationTime)
            {
                GameManager.Inst.UI.OnOffSkill(skillNum, false);
            }
            if (DateTime.Now > endTime)
            {
                skill.isUsed = false;
            }
        }
        if (!isShow) return;
        base.LateUpdate();
        ChangeBtnSprite(GameManager.Inst.CurrentUser.goldCoin,skill.price, skill.price * 10, skill.price * 100, true);


    }
    public int CheckCoolTime(DateTime dateTime_1, DateTime dateTime_2)
    {
        if (dateTime_1.Minute == dateTime_2.Minute)
        {
            return dateTime_1.Second - dateTime_2.Second;
        }
        else
        {
            if (dateTime_1.Minute < dateTime_2.Minute)
            {
                return 0;
            }
            else
            {
                return ((dateTime_1.Minute - dateTime_2.Minute) * 60 + dateTime_1.Second) - dateTime_2.Second;
            }
        }
    }

    public override void SetPanalNum(int num)
    {
        skill = GameManager.Inst.CurrentUser.skills[num];
        //staffSprite = GameManager.Inst.UI.SoldierSpriteArray[num];
        skillNum = num;
        if (skill.isUsed)
        {
            endTime = DateTime.Parse(skill.endTime);
            endDurationTime = DateTime.Parse(skill.endDurationTime);
            if (DateTime.Now < endDurationTime)
            {
                GameManager.Inst.UI.OnOffSkill(skillNum, true);
            }
        }
    }

    public override void UpdateValues()
    {
        skillNameText.text = string.Format("Lv.{0} {1}", skill.level, skill.skillName);
        ChangeBuyBtnPriceText("골드", skill.price, skill.price * 10, skill.price * 100);
        ChangeBuyBtnInfo("구매"); 
        buyBtnImages[2].sprite = GameManager.Inst.CurrentUser.goldCoin >= skill.price ? GameManager.Inst.UI.BuyBtnSpriteArray[1] : GameManager.Inst.UI.BuyBtnSpriteArray[0];
    }

    public void OnClickUpgradeSkill(int amount)
    {
        if (!UpgradeSkill(amount)) return;

        if (amount != 1)
        {
            timer = 0f;
        }
    }

    private bool UpgradeSkill(int amount)
    {
        if (GameManager.Inst.CurrentUser.goldCoin >= skill.price * amount)
        {
            GameManager.Inst.CurrentUser.goldCoin -= skill.price * amount;
            GameManager.Inst.CurrentUser.levelUpCnt++;
            skill.level += amount;
            UpdateValues();
            GameManager.Inst.UI.UpdateMoneyPanal();
            GameManager.Inst.UI.ShowMessage("구매 완료");
            return true;
        }
        else
        {
            GameManager.Inst.UI.ShowMessage("돈이 부족합니다");
            return false;
        }
    }

    public void OnClickUseSkill()
    {
        if (skill.isUsed) return;
        skill.endTime = DateTime.Now.AddSeconds(skill.coolTime).ToString("G");
        skill.endDurationTime = DateTime.Now.AddSeconds(skill.duration).ToString("G");
        endTime = DateTime.Parse(skill.endTime);
        GameManager.Inst.CurrentUser.skillUseCnt++;
        endDurationTime = DateTime.Parse(skill.endDurationTime);
        GameManager.Inst.UI.UpdateMoneyPanal();
        skill.isUsed = true;
        GameManager.Inst.CurrentUser.clickCnt++;
        GameManager.Inst.UI.OnOffSkill(skillNum, true);
    }


    public override void ShowBulkPurchaseBtn()
    {
        base.ShowBulkPurchaseBtn();
    }

    public override void ReloadBulkPurchaseBtn()
    {
        base.ReloadBulkPurchaseBtn();
        ChangeBtnSprite(GameManager.Inst.CurrentUser.goldCoin, skill.price, skill.price * 10, skill.price * 100, false);
    }
}
