using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Animator beakerAnimator = null;
    [SerializeField] private Text energyText = null;
    [SerializeField] private GameObject upgradePanalTemp = null;

    private List<UpgradePanal> upgradePanalList = new List<UpgradePanal>();
    private void Start()
    {
        UpdateEnergyPanal();
        CreatePanals();
    }

    private void CreatePanals()
    {

        GameObject newPanal = null;
        UpgradePanal newUpgradePanal = null;

        foreach(Soldier soldier in GameManager.Inst.CurrentUser.soldiers)
        {
            newPanal = Instantiate(upgradePanalTemp, upgradePanalTemp.transform.parent);
            newUpgradePanal = newPanal.GetComponent<UpgradePanal>();
            newUpgradePanal.SetValue(soldier);
            newPanal.SetActive(true);
        }
    }
    public void OnClickBeaker()
    {
        GameManager.Inst.CurrentUser.energy += GameManager.Inst.CurrentUser.ePc;
        beakerAnimator.Play("ClickAnim");
        UpdateEnergyPanal();
    }

    public void OnClickPurchaseBtn()
    {
        //GameManager.Inst.CurrentUser.soldiers.Find(x => x == );
    }

    public void UpdateEnergyPanal()
    {
        energyText.text = string.Format("{0} ¿¡³ÊÁö", GameManager.Inst.CurrentUser.energy);
    }
}
