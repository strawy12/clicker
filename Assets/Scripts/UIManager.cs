using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using DG.Tweening;
//using UnityEngine.ResourceManagement.AsyncOperations;
//using UnityEngine.AddressableAssets;
using BigInteger = System.Numerics.BigInteger;


public class UIManager : MonoBehaviour
{
    [Header("캐릭터용")]
    [SerializeField] private SpriteRenderer characterSpriteRenderer = null;


    [Header("시스템")]
    [SerializeField] private ScrollRect scrollRect = null;
    [SerializeField] private GameObject moneyImage = null;
    [SerializeField] private GameObject coinImage = null;
    [SerializeField] private RectTransform buffs = null;
    [SerializeField] private GameObject systemBtns = null;
    [SerializeField] private Slider bgmController = null;
    [SerializeField] private Slider effectController = null;
    [SerializeField] GameObject fpsBtns = null;


    [Header("프리팹")]
    [SerializeField] private GameObject staffPanalTemp = null;
    [SerializeField] private GameObject companyPanalTemp = null;
    [SerializeField] private GameObject petPanalTemp = null;
    [SerializeField] private Button staffObjectTemp = null;
    [SerializeField] private CoinText coinTextTemp = null;
    [SerializeField] private CottenCandy conttenCandyTemp = null;
    [SerializeField] private GameObject clickEffectTemp = null;
    [SerializeField] private Image randomPickTemp = null;

    [SerializeField] private GameObject messageObject = null;


    [Header("패널들")]
    [SerializeField] private GameObject settingPanal = null;
    [SerializeField] private GameObject staffObjPanal = null;
    [SerializeField] private RectTransform controlPanal = null;
    [SerializeField] private GameObject missionPanal = null;
    [SerializeField] private PetInfo petinfoPanal = null;
    [SerializeField] private Image randomPickPanal = null;
    [SerializeField] private GameObject rewardPanal = null;
    [SerializeField] private GameObject helpPanal = null;
    [SerializeField] private UpgradeSahyangPanal sahyangPanal = null;
    [SerializeField] GameObject selectingPanal = null;
    [SerializeField] InFoPanal inFoPanal = null;
    [SerializeField] IllustratedBookPanal iBookPanal = null;
    [SerializeField] GameObject quitPanal = null;

    [Header("돈")]
    [SerializeField] private Text moneyText = null;
    [SerializeField] private Text goldCoinText = null;

    [Header("컨텐츠 배열")]
    [SerializeField] private RectTransform[] scrollObject = null;
    [SerializeField] private GameObject[] staffObjects = null;

    private Text rewardText = null;
    private MissionPanal[] missionPanalArray = null;
    private Sprite[] staffSprites = null;
    private Sprite[] buyBtnSprites = null;
    private Sprite[] skillSprites = null;
    private Sprite[] petSprites = null;
    private Sprite[] missionSprites = null;
    private List<GameObject> randomPickObj = new List<GameObject>();
    private Coroutine messageCo = null;
    public Button[] seletingBtns { get; private set; } = null;
    private Button[] fpsSettingBtns = null;
    private Image[] systemBtnImages = null;

    private Text messageText = null;
    private bool isPicking = false;

    private bool isShowing = false;
    public Sprite[] StaffSpriteArray { get { return staffSprites; } }
    public Sprite[] PetSpriteArray { get { return petSprites; } }
    public Sprite[] BuyBtnSpriteArray { get { return buyBtnSprites; } }
    public Sprite[] SkillSpriteArray { get { return skillSprites; } }
    public Sprite[] MissionSpriteArray { get { return missionSprites; } }

    public bool isShow { get; private set; } = false;
    private bool isShow_Setting = false;
    private bool isAdditionClick = false;
    public int scrollNum { get; private set; }
    private int additionClickCnt = 0;
    private int clickCnt = 0;
    private int randNum;
    private int nowMissionClearCnt = 0;
    private int iBookCnt = 0;

    private string spritePath = "StaffSprites";
    private string spriteUIPath = "Clicker Button UI";
    private string skillSpritePath = "SkillImage";
    private string petSpritePath = "Pet Animal";

    private Canvas canvas;

    private List<UpgradePanalBase> upgradePanalList = new List<UpgradePanalBase>();

    private void Awake()
    {
        //AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>(spritePath);
        //spriteHandle.Completed += LoadSpriteWhenReady;
        buyBtnSprites = Resources.LoadAll<Sprite>(spriteUIPath);
        staffSprites = Resources.LoadAll<Sprite>(spritePath);
        petSprites = Resources.LoadAll<Sprite>(petSpritePath);
        skillSprites = Resources.LoadAll<Sprite>(skillSpritePath);
        messageText = messageObject.transform.GetChild(0).GetComponent<Text>();
        missionPanalArray = missionPanal.GetComponentsInChildren<MissionPanal>();
        canvas = FindObjectOfType<Canvas>();
        AddSystemBtn();
        fpsSettingBtns = fpsBtns.GetComponentsInChildren<Button>();
        rewardText = rewardPanal.transform.GetChild(1).GetComponent<Text>();
        seletingBtns = selectingPanal.transform.GetComponentsInChildren<Button>();
    }

    private void Update()
    {
        if (GameManager.Inst.CurrentUser.playTime < 1800f)
        {
            GameManager.Inst.CurrentUser.playTime += Time.deltaTime;
        }
    }

    private void Start()
    {
        isShow = false;
        bgmController.value = GameManager.Inst.CurrentUser.bgmVolume;
        effectController.value = GameManager.Inst.CurrentUser.effectVolume;
        randNum = Random.Range(120, 200);
        MissionPanalGoStart();
        CreatePanals();
        UpdateMoneyPanal();
        SetScrollActive(1);
    }

    public void OnClickCheetBtn()
    {
        GameManager.Inst.CurrentUser.UpdateMoney(1000000000000, true);
        GameManager.Inst.CurrentUser.goldCoin += 1000;
    }

    private void AddSystemBtn()
    {
        List<Image> images = new List<Image>();

        for (int i = 0; i < systemBtns.transform.childCount - 1; i++)
        {
            images.Add(systemBtns.transform.GetChild(i).GetChild(1).GetComponent<Image>());
        }
        systemBtnImages = images.ToArray();
    }

    private void MissionPanalGoStart()
    {
        foreach (MissionPanal missionPanal in missionPanalArray)
        {
            missionPanal.GoStart();
        }
    }

    public List<T> Mix<T>(List<T> pets)
    {
        List<T> list = new List<T>();
        int count = pets.Count;

        for (int i = 0; i < count; i++)
        {
            int rand = Random.Range(0, pets.Count);
            list.Add(pets[rand]);
            pets.RemoveAt(rand);
        }
        return list;
    }


    private void CreatePanals()
    {
        GameObject newPanal = null;
        UpgradePanalBase newUpgradePanal = null;

        foreach (Staff staff in GameManager.Inst.CurrentUser.staffs)
        {
            newPanal = Instantiate(staffPanalTemp, staffPanalTemp.transform.parent);
            newUpgradePanal = newPanal.GetComponent<UpgradePanalBase>();
            upgradePanalList.Add(newUpgradePanal);
            iBookPanal.SpawnillustratedBook(staff);
            newUpgradePanal.SetPanalNum(staff.staffNum);
            newPanal.SetActive(true);
            ActiveStaffObj(staff.isSold, staff.staffNum);
        }

        foreach (Skill skill in GameManager.Inst.CurrentUser.skills)
        {

            newPanal = Instantiate(companyPanalTemp, companyPanalTemp.transform.parent);
            newUpgradePanal = newPanal.GetComponent<UpgradePanalBase>();
            upgradePanalList.Add(newUpgradePanal);
            newUpgradePanal.SetPanalNum(skill.skillNum);
            newPanal.SetActive(true);
        }

        foreach (Pet pet in GameManager.Inst.CurrentUser.pets)
        {
            newPanal = Instantiate(petPanalTemp, petPanalTemp.transform.parent);
            newUpgradePanal = newPanal.GetComponent<UpgradePanalBase>();
            upgradePanalList.Add(newUpgradePanal);
            iBookPanal.SpawnillustratedBook(pet);
            newUpgradePanal.SetPanalNum(pet.petNum);
            newPanal.SetActive(true);
        }
        sahyangPanal.SetPanalSahayng();
        upgradePanalList.Add(sahyangPanal);
    }

    public void ActiveStaffObj(bool isShow, int num)
    {
        staffObjects[num].SetActive(isShow);
    }
    private void CheckBigHeart()
    {
        if (clickCnt >= randNum)
        {
            clickCnt = 0;
            randNum = Random.Range(120, 200);
            //ShowSomSaTang();
        }

    }

    public void OnClickDisPlay()
    {
        GameManager.Inst.CurrentUser.clickCnt++;
        if (isAdditionClick)
        {
            additionClickCnt++;
            if (additionClickCnt > 3)
            {
                additionClickCnt = 0;
                StartCoroutine(AdditionClick());
            }
        }
        clickCnt++;
        CheckBigHeart();
        //CheckSpawnPresent();
        GameManager.Inst.CurrentUser.UpdateMoney(GameManager.Inst.CurrentUser.mPc, true);
        UpdateMoneyPanal();
        ShowClickEffect(GameManager.Inst.MousePos);
        ShowCoinText(GameManager.Inst.CurrentUser.mPc);
        StartCoroutine(PopDOTObj(moneyImage));
        StartCoroutine(PopDOTObj(coinImage, 0.07f));
    }

    public void ShowRewardPanal(BigInteger money)
    {
        if (money <= 0) return;
        rewardText.text = string.Format("+ {0}", GameManager.Inst.MoneyUnitConversion(money));
        ShowPanal(rewardPanal);
    }

    public void ShowClickEffect(Vector3 pos)
    {
        //GameObject clickEffect = null;
        //Image clickImage = null;
        //Queue<GameObject> queue = GameManager.Inst.PoolingList[EPoolingType.ClickEffect];
        //if (queue.Count > 0)
        //{
        //    clickEffect = queue.Dequeue();
        //    clickEffect.transform.SetParent(clickEffectTemp.transform.parent);
        //}
        //else
        //{
        //    clickEffect = Instantiate(clickEffectTemp, clickEffectTemp.transform.parent);
        //}
        //clickImage = clickEffect.GetComponent<Image>();
        //clickEffect.transform.position = pos;

        //clickEffect.SetActive(true);
        //clickImage.DOFade(0f, 0.5f).SetEase(Ease.InCirc);
        //clickEffect.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f).SetEase(Ease.OutCirc).OnComplete(() =>
        //{
        //    clickEffect.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f).SetEase(Ease.InCirc).OnComplete(() =>
        //    {
        //        clickImage.DOFade(1f, 0f);
        //        clickEffect.SetActive(false);
        //        clickEffect.transform.localScale = (Vector3.zero);
        //        queue.Enqueue(clickEffect);
        //    });
        //});
    }
    public void ShowSomSaTang()
    {
        CottenCandy somSaTang = GameManager.Inst.GetPoolObject<CottenCandy>();
        CoinText coinText = GameManager.Inst.GetPoolObject<CoinText>();
        ClickEffect clickEffect = GameManager.Inst.GetPoolObject<ClickEffect>();
        coinText.Show(100);
        somSaTang.ClickSomSatang();
        Debug.Log(somSaTang);
        somSaTang.gameObject.SetActive(true);
    }

    public void ShowSelectingPanal(bool isShow)
    {
        ShowPanal(selectingPanal, isShow);
    }
    public void ShowInfoPanal(bool isShow, Staff staff)
    {
        inFoPanal.SetInfo(staff);
        ShowPanal(inFoPanal.gameObject, isShow);
    }
    public void ShowInfoPanal(bool isShow, Pet pet)
    {
        inFoPanal.SetInfo(pet);
        ShowPanal(inFoPanal.gameObject, isShow);
    }

    public void ShowSettingPanal(bool isShow)
    {
        ShowPanal(settingPanal, isShow);
    }

    public void ShowPanal(GameObject panal, bool isShow)
    {
        if(isShow)
        {
            CheckActivePanals(panal);
        }

        if (isShow)
        {

            panal.SetActive(true);
            panal.transform.DOScale(Vector3.one, 0.3f);

        }
        else
        {
            panal.transform.DOScaleX(0f, 0.2f).OnComplete(() => { panal.SetActive(false); panal.transform.localScale = Vector3.zero; });
        }
    }

    private void CheckActivePanals(GameObject panal)
    {
        if(panal == settingPanal || panal == missionPanal || panal == iBookPanal.gameObject || panal == helpPanal)
        {
            if (settingPanal.activeSelf)
            {
                ShowPanal(settingPanal, false);
            }

            if (missionPanal.activeSelf)
            {
                ShowPanal(missionPanal, false);
            }

            if (iBookPanal.gameObject.activeSelf)
            {
                ShowPanal(iBookPanal.gameObject, false);
            }

            if (helpPanal.activeSelf)
            {
                ShowPanal(helpPanal, false);
            }
        }

        else
        {
            return;
        }
        
    }

    public void ShowPanal(GameObject panal)
    {
        CheckActivePanals(panal);
        panal.SetActive(true);
        panal.transform.DOScale(Vector3.one, 0.3f);
    }
    public void UnShowPanal(GameObject panal)
    {
        panal.transform.DOScaleX(0f, 0.2f).OnComplete(() => { panal.SetActive(false); panal.transform.localScale = Vector3.zero; });
    }
    public void UnShowPanal_DoScale(GameObject panal)
    {
        panal.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => { panal.SetActive(false); });
    }

    public void ShowQuitPanal()
    {
        ShowPanal(quitPanal, true);
    }

    public void ShowCoinText(BigInteger money)
    {
        //CoinText coinText = null;
        //Queue<GameObject> queue = GameManager.Inst.PoolingList[EPoolingType.CoinText];
        //if (queue.Count > 0)
        //{
        //    coinText.transform.SetParent(coinTextTemp.transform.parent);
        //}
        //else
        //{
        //    coinText = Instantiate(coinTextTemp, coinTextTemp.transform.parent);
        //}

        //coinText.Show(money);
    }

    public void OnOffSkill(int num, bool isOn)
    {
        switch (num)
        {
            case 0:
                isAdditionClick = isOn;
                break;
            case 1:
                if (isOn)
                {

                    GameManager.Inst.CurrentUser.UpdateMoney(GameManager.Inst.CurrentUser.mPc * GameManager.Inst.CurrentUser.skills[num].level * 1000, true);
                    UpdateMoneyPanal();
                }
                break;
            case 2:
                if (isOn)
                {
                    GameManager.Inst.CurrentUser.additionMoney = 4;
                }
                else
                {
                    GameManager.Inst.CurrentUser.additionMoney = 1;
                }
                break;
        }
    }

    private IEnumerator AdditionClick()
    {
        for (int i = 0; i < 2; i++)
        {
            GameManager.Inst.CurrentUser.UpdateMoney(GameManager.Inst.CurrentUser.mPc, true);
            UpdateMoneyPanal();
            ShowClickEffect(new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-4f, 4f), -5f));
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator PopDOTObj(GameObject obj, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        obj.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        obj.transform.DOScale(Vector3.one, 0.1f);

    }
    public void OnClickRandomPets(int amount)
    {
        if (isPicking)
        {
            SoundManager.Inst.SetEffectSound(1);

            ShowMessage("캐릭터를 이미 뽑고 있습니다.");
            return;
        }
        if (GameManager.Inst.CurrentUser.goldCoin < 100 * amount)
        {
            SoundManager.Inst.SetEffectSound(1);
            ShowMessage("돈이 부족합니다.");
            return;
        }
        GameManager.Inst.CurrentUser.goldCoin -= 100 * amount;
        UpdateMoneyPanal();
        isPicking = true;
        StartCoroutine(RandomPickAnim(amount));
        SoundManager.Inst.SetEffectSound(3);
        ShowMessage("구매 완료");
    }

    private IEnumerator RandomPickAnim(int amount)
    {
        ShowPanal(randomPickPanal.gameObject, true);
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < amount; i++)
        {
            StartCoroutine(RandomPets());
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(0.5f);
        ShowPanal(randomPickPanal.gameObject, false);
        int cnt = randomPickObj.Count;
        for (int i = 0; i < cnt; i++)
        {
            Destroy(randomPickObj[0]);
            randomPickObj.RemoveAt(0);
        }

        isPicking = false;
    }
    public IEnumerator RandomPets()
    {
        int rand = 0;
        int num;
        Pet pet = null;
        List<Pet> list = Mix(GameManager.Inst.CurrentUser.pets.ToList());
        rand = Random.Range(0, 100);
        num = CheckRandPetNum(list, rand);
        pet = GameManager.Inst.CurrentUser.pets[num];
        Image randomPickImage = Instantiate(randomPickTemp, randomPickTemp.transform.parent);
        randomPickObj.Add(randomPickImage.gameObject);
        randomPickImage.gameObject.SetActive(true);
        randomPickImage.DOFade(0f, 0f);
        randomPickImage.sprite = PetSpriteArray[num];
        randomPickImage.DOFade(1f, 1f);
        randomPickImage.rectTransform.DOShakePosition(0.5f);
        randomPickImage.rectTransform.DOShakeScale(0.5f);
        randomPickImage.rectTransform.DOShakeRotation(0.5f);
        SoundManager.Inst.SetEffectSound(4);
        yield return new WaitForSeconds(0.5f);
        randomPickImage.rectTransform.DOScale(new Vector3(1.5f, 1f, 1.5f), 0.2f).OnComplete(() => randomPickImage.rectTransform.DOScale(Vector3.one, 0.1f));

        //SpawnPet(soldierSprites[num], num);

        if (pet.level >= 10)
        {
            GameManager.Inst.CurrentUser.UpdateMoney(num * 1000, true);
        }
        else
        {
            pet.amount++;
            if (pet.amount == 1)
            {
                pet.level++;
            }
        }
        UpdateMoneyPanal();
    }

    public int CheckRandPetNum(List<Pet> petList, int num)
    {
        int cnt = 0;
        for (int i = 0; i < petList.Count; i++)
        {
            if (cnt <= num && num < (cnt + petList[i].percent))
            {
                return petList[i].petNum;
            }
            cnt += petList[i].percent;
        }
        return 0;

    }

    public void SettingFPS(int fps)
    {
        int num = fps / 15 - 2;

        fpsBtns.transform.parent.gameObject.SetActive(true);

        for (int i = 0; i < fpsSettingBtns.Length; i++)
        {
            fpsSettingBtns[i].interactable = true;
        }
        fpsSettingBtns[num].interactable = false;

        fpsBtns.transform.parent.gameObject.SetActive(false);
    }

    public void CheckSelectingBtn(bool isPossive, int num)
    {
        SettingSeletingBtn(false, num);
        if (isPossive)
        {
            GameManager.Inst.Tutorial.SettingPart(num);

        }
        else
        {
            GameManager.Inst.CurrentUser.isTuto[num] = true;
        }
        ShowSelectingPanal(false);

    }

    public void SettingSeletingBtn(bool isAdd, int num)
    {
        if (isAdd)
        {
            GameManager.Inst.UI.seletingBtns[0].onClick.AddListener(() => CheckSelectingBtn(true, num));
            GameManager.Inst.UI.seletingBtns[1].onClick.AddListener(() => CheckSelectingBtn(false, num));
        }

        else
        {
            GameManager.Inst.UI.seletingBtns[0].onClick.RemoveListener(() => CheckSelectingBtn(true, num));
            GameManager.Inst.UI.seletingBtns[1].onClick.RemoveListener(() => CheckSelectingBtn(false, num));
        }
    }

    public void OnClickShowBtn(int num)
    {
        if (isShowing) return;

        

        if (GameManager.Inst.isTutorial)
        {
            if (num != Mathf.Max(GameManager.Inst.Tutorial.progressPartNum - 2, 0))
            {
                return;
            }
        }
        if (!GameManager.Inst.CurrentUser.isTuto[num + 1] && num == 1 && !GameManager.Inst.isTutorial)
        {
            isShowing = true;
            SettingSeletingBtn(true, num + 1);
            ShowSelectingPanal(true);
            controlPanal.DOAnchorPosY(-242f, 0.2f).SetEase(Ease.InCirc).OnComplete(() => isShowing = false);
            buffs.DOAnchorPosY(-140f, 0.2f).SetEase(Ease.InCirc);
            isShow = false;
            return;
        }

        if (isShow && scrollNum != num)
        {
            scrollNum = num;
            SetScrollActive(num);
            return;
        }
        scrollNum = num;
        isShow = !isShow;

        StartCoroutine(ActivePanal(num));
    }

    public IEnumerator ActivePanal(int num)
    {
        isShowing = true;
        if (!isShow)
        {
            controlPanal.DOAnchorPosY(-242f, 0.2f).SetEase(Ease.InCirc).OnComplete(() => isShowing = false);
            buffs.DOAnchorPosY(-149f, 0.2f).SetEase(Ease.InCirc);
            yield return new WaitForSeconds(0.2f);
            staffObjPanal.SetActive(true);
            ShowPanal(staffObjPanal, true);
        }
        else
        {
            staffObjPanal.transform.DOScaleY(0f, 0.2f).OnComplete(() =>
            {
                ShowPanal(staffObjPanal, false);
                staffObjPanal.transform.localScale = Vector3.zero;
            });

            yield return new WaitForSeconds(0.2f);
            controlPanal.DOAnchorPosY(27f, 0.2f).SetEase(Ease.InCirc).OnComplete(() => isShowing = false);
            buffs.DOAnchorPosY(-115f, 0.2f).SetEase(Ease.InCirc);

        }


        SetScrollActive(num);
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
            if(scrollObject[i].transform.localScale != Vector3.one)
            {
                scrollObject[i].transform.localScale = Vector3.one;
            }

            if (i == num)
            {
                scrollObject[i].gameObject.SetActive(true);
                scrollRect.content = scrollObject[i];
                continue;
            }
            scrollObject[i].gameObject.SetActive(false);
        }
    }
    public void UpdateMoneyPanal()
    {
        moneyText.text = string.Format("{0}원", GameManager.Inst.MoneyUnitConversion(GameManager.Inst.CurrentUser.money));
        goldCoinText.text = string.Format("{0} 골드", GameManager.Inst.CurrentUser.goldCoin);
        foreach (UpgradePanalBase upgradePanal in upgradePanalList)
        {
            upgradePanal.UpdateValues();
        }
        CheckMissionClear();
        iBookPanal.UpdateIBook();
    }

    public void CheckMissionClear()
    {
        int cnt = 0;
        for (int i = 0; i < GameManager.Inst.CurrentUser.missions.Length; i++)
        {
            if (GameManager.Inst.CurrentUser.missions[i] && !GameManager.Inst.CurrentUser.missionsClear[i])
            {
                cnt++;
            }
        }
        if (cnt != 0)
        {
            ShowNewMisstionClear(true);
        }
        else
        {
            ShowNewMisstionClear(false);
        }
    }
    public void ShowNewMisstionClear(bool isShow)
    {
        systemBtnImages[0].gameObject.SetActive(isShow);
    }
    public void ShowNewIBook(bool isShow)
    {
        systemBtnImages[1].gameObject.SetActive(isShow);
    }
    public void ShowPetInfoPanal(int num)
    {
        Staff staff = GameManager.Inst.CurrentUser.staffs[num];
        petinfoPanal.SetInfo(staffSprites[num], staff.staffName, staff.staffNum.ToString());
        petinfoPanal.gameObject.SetActive(true);
    }

    public void ShowMessage(string message, float showTime = 0.3f, float unShowTime = 0.1f, float waitingTime = 0.5f, int fontSize = 26)
    {
        if (messageCo != null)
        {
            StopCoroutine(messageCo);
        }
        messageCo = null;
        messageCo = StartCoroutine(Message(message, showTime, unShowTime, waitingTime, fontSize));
    }

    private IEnumerator Message(string message, float showTime, float unShowTime, float waitingTime, int fontSize)
    {
        messageText.text = message;
        messageText.fontSize = fontSize;
        messageObject.transform.localScale = Vector3.zero;
        messageObject.SetActive(true);
        messageObject.transform.DOScale(Vector3.one, showTime);
        yield return new WaitForSeconds(waitingTime);
        messageObject.transform.DOScale(Vector3.zero, unShowTime);
        yield return new WaitForSeconds(unShowTime);
        messageObject.SetActive(false);

        messageCo = null;
    }


}