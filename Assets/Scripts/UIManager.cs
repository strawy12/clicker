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
    [SerializeField] private GameObject settingPanal = null;
    [SerializeField] private RectTransform controlPanal = null;
    [SerializeField] private GameObject compamySystemPanal = null;
    [SerializeField] private GameObject messageObject = null;
    [SerializeField] private GameObject clickPrefab = null;
    [SerializeField] private ScrollRect scrollRect = null;
    private Text messageText = null;
    [SerializeField] private GameObject[] scrollObject = null;
        [SerializeField] private GameObject[] slaveTemp = null;



    private bool isShow = false;
    private bool isShow_Setting = false;
    private int scrollNum;
    private int clickNum = 0;
    private int randNum;

    private Canvas canvas;

    private List<UpgradePanal> upgradePanalList = new List<UpgradePanal>();
    public static Vector3 MousePos
    {
        get
        {
            Vector3 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            result.x = Mathf.Clamp(result.x, GameManager.Inst.MinPos.x, GameManager.Inst.MaxPos.x);
            result.y = Mathf.Clamp(result.y, GameManager.Inst.MinPos.y, GameManager.Inst.MaxPos.y);
            result.z = -10;
            return result;

        }
    }

    private void Start()
    {
        UpdateEnergyPanal();
        CreatePanals();
        SetScrollActive(scrollObject.Length);
        isShow = false;
        messageText = messageObject.transform.GetChild(0).GetComponent<Text>();
        randNum = Random.Range(230, 500);
        canvas = FindObjectOfType<Canvas>();
    }


    private void CreatePanals()
    {
        GameObject newPanal = null;
        UpgradePanal newUpgradePanal = null;

        for (int i = 0; i < GameManager.Inst.CurrentUser.soldiers.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject panal = j == 0 ? upgradePanalTemp : slavePanalTemp;
                newPanal = Instantiate(panal, panal.transform.parent);
                newUpgradePanal = newPanal.GetComponent<UpgradePanal>();
                newUpgradePanal.SetSoldierNum(i);
                newPanal.SetActive(true);
            }
            if (GameManager.Inst.CurrentUser.soldiers[i].amount != 0)
            {
                ActiveCompanySystemPanal(i, true);
                continue;
            }
            ActiveCompanySystemPanal(i, false);
        }
    }

    public void ActiveCompanySystemPanal(int num, bool isActive)
    {
        compamySystemPanal.transform.GetChild(num).gameObject.SetActive(isActive);
    }

    private void CheckSpawnPresent()
    {
        if (clickNum == randNum)
        {
            clickNum = 0;
            randNum = Random.Range(230, 500);
        }

    }

    public void OnClickBeaker()
    {
        clickNum++;
        //CheckSpawnPresent();
        GameManager.Inst.CurrentUser.money += GameManager.Inst.CurrentUser.mpc;
        SpawnClickText(GameManager.Inst.CurrentUser.mpc);
        beakerAnimator.Play("ClickAnim");
        UpdateEnergyPanal();
    }

    public void OnClickShowBtn(int num)
    {
        if (isShow && scrollNum != num)
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

    public void SpawnClickText(long num)
    {
        Text text = Instantiate(clickPrefab, clickPrefab.transform.parent).GetComponent<Text>();
        RectTransform rectTransform = text.GetComponent<RectTransform>();
        Image image = text.transform.GetChild(0).GetComponent<Image>();

        text.transform.position = SetPosition(num);
        text.gameObject.SetActive(true);
        text.text = string.Format("+{0}", num);

        float targetPositionY = rectTransform.anchoredPosition.y + 100f;
        rectTransform.DOAnchorPosY(targetPositionY, 0.5f);
        text.DOFade(0f, 0.5f).OnComplete(() => text.DOFade(1f, 0f).OnComplete(() => DestroyText(text.gameObject)));
        image.DOFade(0f, 0.5f).OnComplete(() => text.DOFade(1f, 0f));
    }

    private void DestroyText(GameObject text)
    {
        Destroy(text);
    }

    private Vector3 SetPosition(long num)
    {
        if (MousePos.x < GameManager.Inst.MaxPos.x) return MousePos;
        int digit = (int)Mathf.Log10(num) + 1;
        Vector3 targetPos = MousePos;
        switch (digit)
        {
            case 1:
                targetPos.x = 1.94f;
                break;
            case 2:
                targetPos.x = 1.8f;
                break;
            case 3:
                targetPos.x = 1.7f;
                break;
            case 4:
                targetPos.x = 1.6f;
                break;
        }
        return targetPos;
    }


    public void OnClickSettingBtn()
    {
        isShow_Setting = !isShow_Setting;
        settingPanal.SetActive(isShow_Setting);
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
        energyText.text = string.Format("{0} 에너지", GameManager.Inst.CurrentUser.money);
    }
    public void SpawnJJikJJikE(Sprite soldierSprite, int num)
    {
        Debug.Log("응애");
        GameObject slave = Instantiate(slaveTemp[num], slaveTemp[num].transform.parent);
        slave.GetComponent<Image>().sprite = soldierSprite;
        slave.SetActive(true);
    }

    public IEnumerator Message(UpgradePanal upgrade, string message)
    {
        messageText.text = message;

        messageObject.transform.localScale = Vector3.zero;
        messageObject.transform.DOScale(Vector3.one, 0.3f);
        yield return new WaitForSeconds(0.5f);
        messageObject.transform.DOScale(Vector3.zero, 0.1f);
        upgrade.CoroutineNull();
    }
}
