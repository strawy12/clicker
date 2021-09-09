using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Animator beakerAnimator = null;
    [SerializeField] private Text energyText = null;
    [SerializeField] private GameObject slavePanalTemp = null;
    [SerializeField] private GameObject upgradePanalTemp = null;
    [SerializeField] private RectTransform controlPanal = null;
    [SerializeField] private GameObject jjikjjikEPrefab = null;
    [SerializeField] private GameObject messageObject = null;
    [SerializeField] private ScrollRect scrollRect = null;
    private Text messageText = null;
    [SerializeField] private GameObject[] scrollObject;


    private bool isShow = false;
    private int scrollNum;


    private List<UpgradePanal> upgradePanalList = new List<UpgradePanal>();


    private void Start()
    {
        UpdateEnergyPanal();
        CreatePanals();
        SetScrollActive(scrollObject.Length);
        isShow = false;
        messageText = messageObject.transform.GetChild(0).GetComponent<Text>();
    }


    private void CreatePanals()
    {
        GameObject newPanal = null;
        UpgradePanal newUpgradePanal = null;

        for(int i = 0; i < GameManager.Inst.CurrentUser.soldiers.Count; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                GameObject panal = j == 0 ? upgradePanalTemp : slavePanalTemp;
                newPanal = Instantiate(panal, panal.transform.parent);
                newUpgradePanal = newPanal.GetComponent<UpgradePanal>();
                newUpgradePanal.SetSoldierNum(i);
                newPanal.SetActive(true);
            }
            
        }
    }
    public void OnClickBeaker()
    {
        
        GameManager.Inst.CurrentUser.money += GameManager.Inst.CurrentUser.mPc * (1 + GameManager.Inst.CurrentUser.peopleCnt);
        beakerAnimator.Play("ClickAnim");
        UpdateEnergyPanal();
    }

    public void OnClickShowBtn(int num)
    {
        if(isShow && scrollNum != num)
        {
            scrollNum = num;
            SetScrollActive(num);
            return;
        }
        scrollNum = num;
        isShow = !isShow;
        controlPanal.DOAnchorPosY(isShow ? 0 : -300f, 0.2f).SetEase(Ease.InCirc);
        SetScrollActive(num);
    }

    private void SetScrollActive(int num)
    {
        for (int i = 0; i < scrollObject.Length; i++)
        {
            if (i == num)
            {
                scrollObject[i].SetActive(true);
                scrollRect.content = scrollObject[i].GetComponent<RectTransform>();
                continue;
            }
            scrollObject[i].SetActive(false);
        }
    }
    public void UpdateEnergyPanal()
    {
        energyText.text = string.Format("{0} ¿¡³ÊÁö", GameManager.Inst.CurrentUser.money);
    }
    public void SpawnJJikJJikE(Sprite soldierSprite)
    {
        float x = Random.Range(-1.7f, 1.7f);
        float y = Random.Range(0f, -3.6f);

        Instantiate(jjikjjikEPrefab, new Vector2(x, y), Quaternion.identity).GetComponent<SpriteRenderer>().sprite = soldierSprite;

    }

    public IEnumerator Message(string message)
    {
        messageText.text = message;

        messageObject.transform.DOScale(Vector3.one, 0.3f);
        yield return new WaitForSeconds(0.5f);
        messageObject.transform.DOScale(Vector3.zero, 0.1f);
    }
}
