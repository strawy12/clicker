using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ESkillType = GameManager.ESkillType;

public class SkillUpgradePanal : UpgradePanalBase
{
    [SerializeField] private Text skillNameText = null;
    [SerializeField] private Image skillImage = null;
    [SerializeField] private Image coolTime = null;
    private Skill skill = null;
    private int skillNum;
    private DateTime endTime;
    private DateTime endDurationTime;

    public void LateUpdate()
    {
        if(skill.isUsed && skill.skilltype == ESkillType.Active)
        {
            Debug.Log(skill.coolTime);
            Debug.Log(skill.duration);
            coolTime.fillAmount = (float)CheckCoolTime(endTime, DateTime.Now) / skill.coolTime;
            if(DateTime.Now > endDurationTime)
            {
                GameManager.Inst.UI.OnOffSkill(skillNum, false);
            }
            if (DateTime.Now > endTime)
            {
                skill.isUsed = false;
            }
        }
    }
    public int CheckCoolTime(DateTime dateTime_1, DateTime dateTime_2)
    {
        if(dateTime_1.Minute == dateTime_2.Minute)
        {
            return dateTime_1.Second - dateTime_2.Second;
        }
        else
        {
            if(dateTime_1.Minute < dateTime_2.Minute)
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
        if(skill.isUsed)
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
        base.UpdateValues();
        skillNameText.text = skill.skillName;
        priceText.text = string.Format("{0} 원", skill.price);
        //staffImage.sprite = staffSprite;
    }

    public void OnClickUpgradeSkill()
    {
        if (GameManager.Inst.CurrentUser.money >= skill.price)
        {
            GameManager.Inst.CurrentUser.money -= skill.price;
            skill.level++;
            skill.price = (long)(skill.price * 1.25f);
            UpdateValues();
            GameManager.Inst.UI.UpdateMoneyPanal();
            GameManager.Inst.UI.ShowMessage("구매 완료");
        }
        else
        {
            GameManager.Inst.UI.ShowMessage("돈이 부족합니다");
        }
    }

    public void OnClickUseSkill()
    {
        if (skill.skilltype != ESkillType.Active) return;
        if (skill.isUsed) return;
        skill.endTime = DateTime.Now.AddSeconds(skill.coolTime).ToString("G");
        skill.endDurationTime = DateTime.Now.AddSeconds(skill.duration).ToString("G");
        endTime = DateTime.Parse(skill.endTime);
        endDurationTime = DateTime.Parse(skill.endDurationTime);
        GameManager.Inst.UI.UpdateMoneyPanal();
        skill.isUsed = true;
        GameManager.Inst.UI.OnOffSkill(skillNum, true);
    }
}
