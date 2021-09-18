using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUpgradePanal : UpgradePanalBase
{
    [SerializeField] private Text skillNameText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private Text levelText = null;
    [SerializeField] private Image skillImage = null;

    private Skill skill = null;
    private int skillNum;

    public override void SetPanalNum(int num)
    {
        skill = GameManager.Inst.CurrentUser.skills[num];
        //staffSprite = GameManager.Inst.UI.SoldierSpriteArray[num];
        skillNum = num;
        UpdateValues();
    }

    public override void UpdateValues()
    {
        skillNameText.text = skill.skillName;
        priceText.text = string.Format("{0} ¿ø", skill.price);
        levelText.text = string.Format("Lv. {0}", skill.level);
        //staffImage.sprite = staffSprite;
    }
}
