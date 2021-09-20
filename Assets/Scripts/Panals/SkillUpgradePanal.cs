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
    [SerializeField] private Slider cooltime = null;
    private Skill skill = null;
    private int skillNum;
    private DateTime endTime;

    public void LateUpdate()
    {
        if(skill.isUsed && skill.skilltype == ESkillType.Active)
        {
            cooltime.value = (float) endTime.Second - DateTime.Now.Second / skill.coolTime;
            if (DateTime.Now > endTime)
            {
                Debug.Log("ÀÀ¾Ö");
                skill.isUsed = false;
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
        }
    }

    public override void UpdateValues()
    {
        base.UpdateValues();
        skillNameText.text = skill.skillName;
        priceText.text = string.Format("{0} ¿ø", skill.price);
        //staffImage.sprite = staffSprite;
    }
    public void OnClickUseSkill()
    {
        if (skill.skilltype != ESkillType.Active) return;
        if (skill.isUsed) return;
        skill.endTime = DateTime.Now.AddSeconds(skill.coolTime).ToString("G");
        endTime = DateTime.Parse(skill.endTime);
        GameManager.Inst.CurrentUser.money += 1000000;
        GameManager.Inst.UI.UpdateMoneyPanal();
        skill.isUsed = true;
    }
}
