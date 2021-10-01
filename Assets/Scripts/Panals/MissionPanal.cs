using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MissionPanal : MonoBehaviour
{
    [SerializeField] private Image missionBtnImage = null;
    [SerializeField] private Slider missionComplateBar = null;
    [SerializeField] private Text missionSliderText = null;
    [SerializeField] private Text missionBtnText = null;

    [SerializeField] private int missionNum;
    private bool CanReceive
    {
        get
        {
            if(missionNum == 5)
            {
                return GameManager.Inst.CurrentUser.missionClear == 5 && !GameManager.Inst.CurrentUser.missionsClear[5];
            }

            else if(GameManager.Inst.CurrentUser.missions[missionNum] && !GameManager.Inst.CurrentUser.missionsClear[missionNum])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    
    
    public void GoStart()
    {
        UpdatePanal();
    }

    public void Update()
    {
        if(gameObject.activeSelf)
        {
            UpdatePanal();
        }
    }

    private void OnEnable()
    {
        GameManager.Inst.UI.ShowNewMisstionClear(false);    
        UpdatePanal();
    }

    void UpdatePanal()
    {
        missionBtnImage.sprite = GameManager.Inst.UI.BuyBtnSpriteArray[CanReceive ? 1 : 0];
        switch (missionNum)
        {
            case 0:
                missionComplateBar.value = GameManager.Inst.CurrentUser.clickCnt / 2000;
                missionSliderText.text = string.Format("{0} / {1}", GameManager.Inst.CurrentUser.clickCnt, 2000);
                break;
            case 1:
                missionComplateBar.value = GameManager.Inst.CurrentUser.bigHeartClickCnt / 30;
                missionSliderText.text = string.Format("{0} / {1}", GameManager.Inst.CurrentUser.bigHeartClickCnt, 30);
                break;
            case 2:
                missionComplateBar.value = GameManager.Inst.CurrentUser.skillUseCnt / 5;
                missionSliderText.text = string.Format("{0} / {1}", GameManager.Inst.CurrentUser.skillUseCnt, 5);
                break;
            case 3:
                missionComplateBar.value = GameManager.Inst.CurrentUser.playTime / 1800f;
                missionSliderText.text = string.Format("{0} / {1}", (int)(GameManager.Inst.CurrentUser.playTime / 60f), 30);
                break;
            case 4:
                missionComplateBar.value = GameManager.Inst.CurrentUser.levelUpCnt / 5;
                missionSliderText.text = string.Format("{0} / {1}", GameManager.Inst.CurrentUser.levelUpCnt, 5);
                break;
            case 5:
                missionComplateBar.value = GameManager.Inst.CurrentUser.missionClear / 5;
                missionSliderText.text = string.Format("{0} / {1}", GameManager.Inst.CurrentUser.missionClear, 5);
                missionBtnImage.sprite = GameManager.Inst.UI.BuyBtnSpriteArray[CanReceive ? 1 : 0];
                break;
        }    
    }

    public void OnClickReceive(int goldCoinAmount)
    {
        if(CanReceive)
        {
            SoundManager.Inst.SetEffectSound(2);
            GameManager.Inst.CurrentUser.goldCoin += goldCoinAmount;
            GameManager.Inst.CurrentUser.missionsClear[missionNum] = true;
        }
        else
        {
            SoundManager.Inst.SetEffectSound(1);

        }
    }
}
