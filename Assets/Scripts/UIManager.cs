using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Animator beakerAnimator = null;
    [SerializeField] private Text energyText = null;
    [SerializeField] private GameObject upgradePanalTemp = null;
    [SerializeField] private RectTransform controlPanal = null;

    private bool isShow = false;

    private List<UpgradePanal> upgradePanalList = new List<UpgradePanal>();
    private void Start()
    {
        UpdateEnergyPanal();
        CreatePanals();
        isShow = false;
    }

    private void CreatePanals()
    {

        GameObject newPanal = null;
        UpgradePanal newUpgradePanal = null;

        for(int i = 0; i < GameManager.Inst.CurrentUser.soldiers.Count; i++)
        {
            newPanal = Instantiate(upgradePanalTemp, upgradePanalTemp.transform.parent);
            newUpgradePanal = newPanal.GetComponent<UpgradePanal>();
            newUpgradePanal.SetSoldierNum(i);
            newPanal.SetActive(true);
        }
    }
    public void OnClickBeaker()
    {
        GameManager.Inst.CurrentUser.money += GameManager.Inst.CurrentUser.mPc;
        beakerAnimator.Play("ClickAnim");
        UpdateEnergyPanal();
    }

    public void OnClickShowBtn()
    {
        isShow = !isShow;
        controlPanal.DOAnchorPosY(isShow ? 0 : -300f, 0.2f).SetEase(Ease.InCirc);
    }

    public void UpdateEnergyPanal()
    {
        energyText.text = string.Format("{0} ¿¡³ÊÁö", GameManager.Inst.CurrentUser.money);
    }
}
