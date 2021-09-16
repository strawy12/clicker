using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class UIManager : MonoBehaviour
{
    [Header("캐릭터용")]
    [SerializeField] private Animator characterAnimator = null;

    [Header("시스템")]
    [SerializeField] private ScrollRect scrollRect = null;
    [SerializeField] private Text randomText = null;

    [Header("프리팹")]
    [SerializeField] private GameObject staffPanalTemp = null;
    [SerializeField] private GameObject staffObjectTemp = null;
    [SerializeField] private CoinText coinTextTemp = null;

    [SerializeField] private GameObject messageObject = null;


    [Header("패널들")]
    [SerializeField] private GameObject settingPanal = null;
    [SerializeField] private RectTransform controlPanal = null;
    [SerializeField] private GameObject compamySystemPanal = null;

    [Header("돈")]
    [SerializeField] private Text moneyText = null;
    [SerializeField] private Text mileageText = null;
    
    [Header("컨텐츠 스크롤")]
    [SerializeField] private GameObject[] scrollObject = null;

    private Sprite[] soldierSprites = null;

    private Coroutine messageCo = null;

    private Text messageText = null;
    private bool isPicking = false;
    public Sprite[] SoldierSpriteArray { get { return soldierSprites; } }

    private bool isShow = false;
    private bool isShow_Setting = false;
    private int scrollNum;
    private int clickNum = 0;
    private int randNum;

    private string spritePath = "Assets/Images/SahyangClickerSoldier.png";

    private Canvas canvas;

    private List<UpgradePanal> upgradePanalList = new List<UpgradePanal>();

    private void Awake()
    {
        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>(spritePath);
        spriteHandle.Completed += LoadSpriteWhenReady;
    }
    private void LoadSpriteWhenReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    {
        if (handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            soldierSprites = handleToCheck.Result;
        }
        GameStartStart();
    }

    private void GameStartStart()
    {

        UpdateMoneyPanal();
        CreatePanals();
        SetScrollActive(scrollObject.Length);
        isShow = false;
        messageText = messageObject.transform.GetChild(0).GetComponent<Text>();
        randNum = Random.Range(230, 500);
        canvas = FindObjectOfType<Canvas>();
    }

    private void Update()
    {

    }

    public List<Staff> Mix(List<Staff> staffs)
    {
        List<Staff> list = new List<Staff>();
        int count = staffs.Count;

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, staffs.Count);
            list.Add(staffs[rand]);
            staffs.RemoveAt(rand);
        }
        return list;
    }


    private void CreatePanals()
    {
        GameObject newPanal = null;
        UpgradePanal newUpgradePanal = null;

        foreach (Staff soldier in GameManager.Inst.CurrentUser.staffs)
        {

            newPanal = Instantiate(staffPanalTemp, staffPanalTemp.transform.parent);
            newUpgradePanal = newPanal.GetComponent<UpgradePanal>();
            newUpgradePanal.SetSoldierNum(soldier.staffNum);
            newPanal.SetActive(true);   
            if (soldier.amount != 0)
            {
                ActiveCompanySystemPanal(soldier.staffNum, true);
                continue;
            }
            ActiveCompanySystemPanal(soldier.staffNum, false);
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

    public void OnClickDisPlay()
    {
        clickNum++;
        //CheckSpawnPresent();
        GameManager.Inst.CurrentUser.money += GameManager.Inst.CurrentUser.zpc;
        characterAnimator.Play("ClickAnim", -1, 0);
        UpdateMoneyPanal();

        ShowCoinText();
    }
    public void ShowCoinText()
    {
        CoinText coinText = null;

        if (GameManager.Inst.Pool.childCount > 0)
        {
            coinText = GameManager.Inst.Pool.GetChild(0).GetComponent<CoinText>();
            coinText.transform.SetParent(coinTextTemp.transform.parent);
        }
        else
        {
            coinText = Instantiate(coinTextTemp, coinTextTemp.transform.parent);
        }

        coinText.Show();
    }

    public void OnClickRandomStaff()
    {
        if (GameManager.Inst.CurrentUser.maxPeople <= GameManager.Inst.CurrentUser.peopleCnt)
        {
            ShowMessage("직원 수가 너무 많습니다.");
            return;
        }
        if (isPicking)
        {
            ShowMessage("캐릭터를 이미 뽑고 있습니다.");
            return;
        }
        if (GameManager.Inst.CurrentUser.money < 10000)
        {
            ShowMessage("돈이 부족합니다.");
            return;
        }

        GameManager.Inst.CurrentUser.money -= 10000;
        GameManager.Inst.CurrentUser.mileage += 100;
        UpdateMoneyPanal();
        StartCoroutine(RandomStaff());
        isPicking = true;
        ShowMessage("구매 완료");
    }
    public IEnumerator RandomStaff()
    {
        int rand = 0;
        int num;
        List<Staff> list = Mix(GameManager.Inst.CurrentUser.staffs.ToList());
        for (int i = 0; i < 50; i++)
        {
            rand = Random.Range(0, 1000);
            randomText.text = GameManager.Inst.CurrentUser.staffs[CheckRandStaffNum(list, rand)].staffName;
            yield return new WaitForSeconds(0.1f);
        }
        num = CheckRandStaffNum(list, rand);
        randomText.text = GameManager.Inst.CurrentUser.staffs[num].staffName;
        SpawnStaff(soldierSprites[num], num);
        GameManager.Inst.CurrentUser.peopleCnt++;
        GameManager.Inst.CurrentUser.staffs[num].amount++;
        if (GameManager.Inst.CurrentUser.staffs[num].amount == 1)
        {
            ActiveCompanySystemPanal(num, true);
        }

        isPicking = false;
    }

    public int CheckRandStaffNum(List<Staff> soldierList, int num)
    {
        int cnt = 0;
        for (int i = 0; i < soldierList.Count; i++)
        {
            if (cnt <= num && num < (cnt + soldierList[i].percent))
            {
                return soldierList[i].staffNum;
            }
            cnt += soldierList[i].percent;
        }
        return 0;

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

    private void DestroyText(GameObject text)
    {
        Destroy(text);
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
    public void UpdateMoneyPanal()
    {
        moneyText.text = string.Format("{0} 찍", GameManager.Inst.CurrentUser.money);
        mileageText.text = string.Format("{0} 마일리지", GameManager.Inst.CurrentUser.mileage);
    }
    public void SpawnStaff(Sprite staffSprite, int num)
    {
        GameObject staff = Instantiate(staffObjectTemp, scrollObject[1].transform.GetChild(num).GetChild(0).GetChild(0));
        staff.GetComponent<Image>().sprite = staffSprite;
        staff.SetActive(true);
    }

    public void ShowMessage(string message)
    {
        if (messageCo != null)
        {
            StopCoroutine(messageCo);
        }

        messageCo = StartCoroutine(Message(message));
    }
    private IEnumerator Message(string message)
    {
        messageText.text = message;

        messageObject.transform.localScale = Vector3.zero;
        messageObject.transform.DOScale(Vector3.one, 0.3f);
        yield return new WaitForSeconds(0.5f);
        messageObject.transform.DOScale(Vector3.zero, 0.1f);
        messageCo = null;
    }
}
