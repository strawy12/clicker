using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
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
    [SerializeField] private GameObject petPanalTemp = null;
    [SerializeField] private Button staffObjectTemp = null;
    [SerializeField] private CoinText coinTextTemp = null;
    [SerializeField] private GameObject clickEffectTemp = null;

    [SerializeField] private GameObject messageObject = null;


    [Header("패널들")]
    [SerializeField] private GameObject settingPanal = null;
    [SerializeField] private RectTransform controlPanal = null;
    [SerializeField] private PetInfo petinfoPanal = null;
    [SerializeField] private Image randomPickPanal = null;

    [Header("돈")]
    [SerializeField] private Text moneyText = null;
    [SerializeField] private Text mileageText = null;
    
    [Header("컨텐츠 스크롤")]
    [SerializeField] private GameObject[] scrollObject = null;

    private Sprite[] soldierSprites = null;
    private Coroutine messageCo = null;
    private Image randomPickImage = null;

    private Text messageText = null;
    private bool isPicking = false;
    public Sprite[] SoldierSpriteArray { get { return soldierSprites; } }

    private bool isShow = false;
    private bool isShow_Setting = false;
    private bool isAdditionClick = false;
    private int scrollNum;
    private int clickNum = 0;
    private int randNum;

    private string spritePath = "Assets/Images/SahyangClickerSoldier.png";

    private Canvas canvas;

    private List<UpgradePanalBase> upgradePanalList = new List<UpgradePanalBase>();

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
        SetScrollActive(2);
        isShow = false;
        messageText = messageObject.transform.GetChild(0).GetComponent<Text>();
        canvas = FindObjectOfType<Canvas>();
        randomPickImage = randomPickPanal.transform.GetChild(0).GetComponent<Image>();
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
            newUpgradePanal.SetPanalNum(staff.staffNum);
            newPanal.SetActive(true);   
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
            newUpgradePanal.SetPanalNum(pet.petNum);
            newPanal.SetActive(true);
        }

    }

    private void OnClick()
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
        if(isAdditionClick && clickNum > 3)
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

    public void OnOffSkill(int num, bool isOn)
    {
        switch (num)
        {
            case 0:
                isAdditionClick = isOn;
                break;
            case 1:
                if(isOn)
                {
                    Debug.Log("응애");
                    GameManager.Inst.CurrentUser.money += (GameManager.Inst.CurrentUser.mPc * 1000);
                    UpdateMoneyPanal();
                }
                break;
            case 2:
                if(isOn)
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
        for(int i = 0; i < 2; i++)
        {
            GameManager.Inst.CurrentUser.money += GameManager.Inst.CurrentUser.mPc;
            UpdateMoneyPanal();
            ShowClickEffect(new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-4f, 4f), -5f));
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnClickRandomPets()
    {
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
        UpdateMoneyPanal();
        RandomPets();
        isPicking = true;
        ShowMessage("구매 완료");
    }
    public void RandomPets()
    {
        int rand = 0;
        int num;
        Pet pet = null;
        List<Pet> list = Mix(GameManager.Inst.CurrentUser.pets.ToList());
        rand = Random.Range(0, 100);
        num = CheckRandPetNum(list, rand);
        pet = GameManager.Inst.CurrentUser.pets[num];
        randomText.text = pet.petName;
        randomPickImage.DOFade(0f, 0f);
        randomPickPanal.rectTransform.DOScale(Vector3.one, 0.2f).OnComplete(() =>
        {
            randomPickImage.sprite = SoldierSpriteArray[num];
            randomPickImage.DOFade(1f, 1f).OnComplete(() =>
            {
                randomPickImage.rectTransform.DOScale(new Vector3(1.5f, 1f, 1.5f), 0.2f).OnComplete(() =>
                {
                    randomPickImage.rectTransform.DOScale(Vector3.one, 0.1f).OnComplete(() =>
                    {
                        randomPickPanal.rectTransform.DOScale(Vector3.zero, 0.3f);
                        isPicking = false;
                    });
                });
            });
            randomPickImage.rectTransform.DOShakePosition(1f);
            randomPickImage.rectTransform.DOShakeScale(1f);
            randomPickImage.rectTransform.DOShakeRotation(1f);
        });
        
        //SpawnPet(soldierSprites[num], num);

        if (pet.level >= 10)
        {
            GameManager.Inst.CurrentUser.money += num * 1000;
        }
        else
        {
            pet.amount++;
            if(pet.amount == 1)
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
        controlPanal.DOAnchorPosY(isShow ? 27f : -242f, 0.2f).SetEase(Ease.InCirc);
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
        foreach(UpgradePanalBase upgradePanal in upgradePanalList)
        {
            upgradePanal.UpdateValues();
        }
    }

    public void ShowPetInfoPanal(int num)
    {
        Staff staff = GameManager.Inst.CurrentUser.staffs[num];
        petinfoPanal.SetInfo(soldierSprites[num], staff.staffName, staff.staffNum.ToString());
        petinfoPanal.gameObject.SetActive(true);
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
