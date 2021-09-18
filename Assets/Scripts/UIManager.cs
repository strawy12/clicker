using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using EPanalState = GameManager.EPanalState;
using EPoolingType = GameManager.EPoolingType;

public class UIManager : MonoBehaviour
{
    [Header("캐릭터용")]
    [SerializeField] private SpriteRenderer characterSpriteRenderer = null;


    [Header("시스템")]
    [SerializeField] private ScrollRect scrollRect = null;
    [SerializeField] private Text randomText = null;

    [Header("프리팹")]
    [SerializeField] private GameObject staffPanalTemp = null;
    [SerializeField] private GameObject companyPanalTemp = null;
    [SerializeField] private GameObject staffObjectTemp = null;
    [SerializeField] private CoinText coinTextTemp = null;
    [SerializeField] private GameObject clickEffectTemp = null;

    [SerializeField] private GameObject messageObject = null;


    [Header("패널들")]
    [SerializeField] private GameObject settingPanal = null;
    [SerializeField] private RectTransform controlPanal = null;

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
    private bool isUseSkill_1 = false;
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

        foreach (Staff staff in GameManager.Inst.CurrentUser.staffs)
        {

            newPanal = Instantiate(staffPanalTemp, staffPanalTemp.transform.parent);
            newUpgradePanal = newPanal.GetComponent<UpgradePanal>();
            newUpgradePanal.SetPanalNum(staff.staffNum, EPanalState.staff);
            newPanal.SetActive(true);   
        }
        foreach (Skill skill in GameManager.Inst.CurrentUser.skills)
        {

            newPanal = Instantiate(companyPanalTemp, companyPanalTemp.transform.parent);
            newUpgradePanal = newPanal.GetComponent<UpgradePanal>();
            newUpgradePanal.SetPanalNum(skill.skillNum, EPanalState.company);
            newPanal.SetActive(true);
        }

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
        if(isUseSkill_1 && clickNum > 10)
        {
            clickNum = 0;
            StartCoroutine(AdditionClick());
        }    
        //CheckSpawnPresent();
        GameManager.Inst.CurrentUser.money += GameManager.Inst.CurrentUser.mPc;
        UpdateMoneyPanal();
        ShowClickEffect(GameManager.Inst.MousePos);
        //ShowCoinText();
    }


    public void ShowClickEffect(Vector3 pos)
    {
        GameObject clickEffect = null;
        Image clickImage = null;
        Queue<GameObject> queue = GameManager.Inst.PoolingList[EPoolingType.clickEffect];
        if (queue.Count > 0)
        {
            clickEffect = queue.Dequeue();
            clickEffect.transform.SetParent(clickEffectTemp.transform.parent);
        }
        else
        {
            clickEffect = Instantiate(clickEffectTemp, clickEffectTemp.transform.parent);
        }
        clickImage = clickEffect.GetComponent<Image>();
        clickEffect.transform.position = pos;

        clickEffect.SetActive(true);
        clickImage.DOFade(0f, 0.5f).SetEase(Ease.InCirc);
        clickEffect.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            clickEffect.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetEase(Ease.InCirc).OnComplete(() =>
            {
                clickImage.DOFade(1f, 0f);
                clickEffect.SetActive(false);
                clickEffect.transform.localScale = (Vector3.zero);
                clickEffect.transform.SetParent(GameManager.Inst.Pool);
                queue.Enqueue(clickEffect);
            });
        });
    }
    public void ShowCoinText()
    {
        CoinText coinText = null;

        if (GameManager.Inst.Pool.childCount > 0)
        {
            GameManager.Inst.Pool.GetChild(0).GetComponent<CoinText>();
            coinText.transform.SetParent(coinTextTemp.transform.parent);
        }
        else
        {
            coinText = Instantiate(coinTextTemp, coinTextTemp.transform.parent);
        }

        coinText.Show();
    }

    public void OnClickSkill(int num)
    {
        switch (num)
        {
            case 1:
                isUseSkill_1 = true;
                break;
            case 2:
                break;
            case 3:
                break;
        }
        StartCoroutine(Timer());
    }

    private IEnumerator AdditionClick()
    {
        for(int i = 0; i < 2; i++)
        {
            GameManager.Inst.CurrentUser.money += GameManager.Inst.CurrentUser.mPc;
            UpdateMoneyPanal();
            ShowClickEffect(new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-4f, 4f), -5f));
            yield return new WaitForSeconds(0.1f);
        }
    }   
    
    private IEnumerator Timer()
    {
        float time = 30f;
        while(time > 0)
        {
            time -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        isUseSkill_1 = false;
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
        GameObject staff = Instantiate(staffObjectTemp, staffObjectTemp.transform.parent);
        staff.transform.GetChild(0).GetComponent<Image>().sprite = staffSprite;
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
