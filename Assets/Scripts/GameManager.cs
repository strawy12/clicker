using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using BigInteger = System.Numerics.BigInteger;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    public enum EPoolingType { clickEffect, coinText, somSaTang }

    private User user = null;

    private UIManager uiManager = null;

    private TutorialManager tutorialManager = null;

    private Transform pool = null;

    private Dictionary<EPoolingType, Queue<GameObject>> poolingList = new Dictionary<EPoolingType, Queue<GameObject>>();
    public User CurrentUser { get { return user; } }

    public TutorialManager Tutorial
    {
        get
        {
            if (tutorialManager == null)
            {
                tutorialManager = GetComponent<TutorialManager>();
            }
            return tutorialManager;
        }
    }


    public UIManager UI
    {
        get
        {
            if(uiManager == null)
            {
                uiManager = GetComponent<UIManager>();
            }
            return uiManager;
        }
    }

    public Transform Pool
    {
        get
        {
            if (pool == null)
            {
                pool = GameObject.Find("Pool").transform;
            }
            return pool;
        }
    }
    public Dictionary<EPoolingType, Queue<GameObject>> PoolingList { get { return poolingList; } }

    public bool isTutorial = false;

    private string SAVE_PATH = "";

    private const string SAVE_FILENAME = "/SaveFile.txt";

    private string moneyUnits = ",¸¸,¾ï,Á¶,°æ,ÇØ,ÀÀ,¾Ö,Ÿ¶,ÇÏ,ÁØ,Èï,ÇÏ,ÀÚ,Âï";

    public string[] moneyUnit
    {
        get
        {
            return moneyUnits.Split(',');
        }

    }

    private float timer = 0f;


    public Vector2 MaxPos { get; private set; } = new Vector2(2.05f, 4.2f);
    public Vector2 MinPos { get; private set; } = new Vector2(-2.05f, -4.2f);
    public Vector3 MousePos
    {
        get
        {
            Vector3 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            result.x = Mathf.Clamp(result.x, MinPos.x, MaxPos.x);
            result.y = Mathf.Clamp(result.y, MinPos.y, MaxPos.y);
            result.z = -10f;
            return result;
        }
    }

    public void Awake()
    {   
        SAVE_PATH = Application.persistentDataPath + "/Save";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        uiManager = GetComponent<UIManager>();
        tutorialManager = GetComponent<TutorialManager>();
        LoadFromJson();
        Application.targetFrameRate = user.frame;
        SetDict();

    }

    private void Start()
    {
        SoundManager.Inst.VolumeSetting();
        SoundManager.Inst.SetBGM(0);
        SoundManager.Inst.SetEffectSound(0);

        SettingUser();
        uiManager.SettingFPS(user.frame);
        CheckReJoinTime();
        CheckAutoClick();

        InvokeRepeating("SaveToJson", 5f, 60f);
        InvokeRepeating("MoneyPerSecond", 5f, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.ShowQuitPanal();
        }
            timer += Time.deltaTime;
            if(timer >= user.autoClickTime)
            {
                AutoClick();
                timer = 0f;
            }
        
    }

    private void CheckAutoClick()
    {
        if(user.autoClickUsingTime == "" || user.autoClickUsingTime == null)
        {
            timer = user.autoClickTime;
            return;
        }
        TimeSpan diff = DateTime.Parse(user.autoClickUsingTime) - DateTime.Now;
        if (diff.TotalSeconds > 0)
        {
            timer = user.autoClickTime - (float)diff.TotalSeconds;
        }
        else
        {
            timer = user.autoClickTime;
        }
    }

    private void SetDict()
    {
        poolingList.Add(EPoolingType.clickEffect, new Queue<GameObject>());
        poolingList.Add(EPoolingType.coinText, new Queue<GameObject>());
        poolingList.Add(EPoolingType.somSaTang, new Queue<GameObject>());
    }

    public void Quit()
    {
        SaveToJson();
        Application.Quit();
    }

    public void OnClickDeleteJson()
    {
        uiManager.ShowMessage("µ¥ÀÌÅÍ¸¦ »èÁ¦ÇÕ´Ï´Ù. ÃÊ±âÈ­¸¦ À§ÇØ °ÔÀÓÀ» Á¾·áÇÕ´Ï´Ù.", 0.3f, 0.1f, 3f, 22);
        Invoke("DeleteJson", 3f);
    }

    public void DeleteJson()
    {
        File.Delete(SAVE_PATH + SAVE_FILENAME);
        user = null;
        Quit();
    }
    private void LoadFromJson()
    {
        string json = "";
        if (File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            user = JsonUtility.FromJson<User>(json);
            user.ConversionType(false);

        }
        if (user == null)
        {
            user = new User();
            user.userName = "";
            user.basemPc = 10;
            user.additionMoney = 1;
            user.frame = 60;
            user.goldCoin = 0;
            user.money = 100;
            user.sahayang = new Sahayang();
            user.isTuto = new bool[5];
            user.missionsClear = new bool[6];
            user.sahayang.price = 1000;
            user.sahayang.level = 1;
            user.bgmVolume = 0.5f;
            user.effectVolume = 0.5f;

            user.staffs.Add(new Staff("ÀÀ¾ÖÂïÂïÀÌ", 0, 0, 10, 1000));
            user.staffs.Add(new Staff("¼Ò³âÂïÂïÀÌ", 1, 0, 200, 2000));
            user.staffs.Add(new Staff("¼Ò³àÂïÂïÀÌ", 2, 0, 3000, 30000));
            user.staffs.Add(new Staff("¾Ë¹ÙÂïÂïÀÌ", 3, 0, 20000, 200000));
            user.staffs.Add(new Staff("³óºÎÂïÂïÀÌ", 4, 0, 150000, 1500000));
            user.staffs.Add(new Staff("º¹¼­ÂïÂïÀÌ", 5, 0, 1000000, 10000000));
            user.staffs.Add(new Staff("ÀÇ»çÂïÂïÀÌ", 6, 0, 7000000, 70000000));
            user.staffs.Add(new Staff("¼Ò¹æ°üÂïÂïÀÌ", 7, 0, 15000000, 150000000));
            user.staffs.Add(new Staff("ÇïÃ¢ÂïÂïÀÌ", 8, 0, 30000000, 300000000));
            user.staffs.Add(new Staff("±ºÀÎÂïÂïÀÌ", 9, 0, 50000000, 500000000));
            user.staffs.Add(new Staff("OIFÂïÂïÀÌ", 10, 0, 100000000, 1000000000));

            user.skills.Add(new Skill("ÂøÃëÀÇ ÇöÀå", 0, 1, 100, 30, 100));
            user.skills.Add(new Skill("³ëµ¿ÀÇ ´ë°¡", 1, 1, 100, 0, 200));
            user.skills.Add(new Skill("Áø½É ¸ğµå", 2, 1, 100, 30, 300));

            user.pets.Add(new Pet(0, "°­¾ÆÁö", 0, 0, 20, 1000));
            user.pets.Add(new Pet(1, "Åä³¢", 0, 0, 20, 1000));
            user.pets.Add(new Pet(2, "¿©¿ì", 0, 0, 15, 1000));
            user.pets.Add(new Pet(3, "°õ", 0, 0, 10, 1000));
            user.pets.Add(new Pet(4, "Æë±Ï", 0, 0, 10, 1000));
            user.pets.Add(new Pet(5, "³Ê±¸¸®", 0, 0, 7, 1000));
            user.pets.Add(new Pet(6, "µÅÁö", 0, 0, 7, 1000));
            user.pets.Add(new Pet(7, "¾ç", 0, 0, 5, 1000));
            user.pets.Add(new Pet(8, "´Ù¶÷Áã", 0, 0, 5, 1000));
            user.pets.Add(new Pet(9, "±î¹Ì", 0, 0, 1, 1000));

            SaveToJson();
        }
    }
    public T[] FindImages<T>(GameObject gameObject)
    {
        T[] arr = gameObject.GetComponentsInChildren<T>();
        T[] returnArr = new T[arr.Length - 1];
        for (int i = 1; i < arr.Length; i++)
        {
            returnArr[i - 1] = arr[i];
        }

        return returnArr;
    }

    private void SaveToJson()
    {
        if (user == null) return;
        user.ConversionType(true);
        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
    }

    public bool CheckDate()
    {
        if (user.exitTime != null)
        {
            TimeSpan dateDiff = DateTime.Now - DateTime.Parse(user.exitTime);
            int diffDay = dateDiff.Days;
            if (diffDay == 0) return false;
        }
        return true;
    }

    public BigInteger MultiflyBigInteger(BigInteger num1, float num2, int dight)
    {
        int multiflyNum = (int)Mathf.Pow(10, dight);
        BigInteger nums = (BigInteger)(num2 * multiflyNum);
        BigInteger sum = num1 * nums;

        sum /= multiflyNum;

        return sum;
    }
    private void SettingUser()
    {
        if (CheckDate())
        {
            for(int i = 0; i < user.missionsClear.Length; i++)
            {
                user.missionsClear[i] = false;
            }
            user.playTime = 0f;
            user.skillUseCnt = 0;
            user.clickCnt = 0;
            user.bigHeartClickCnt = 0;
            user.levelUpCnt = 0;
        }
    }

    public void CheckReJoinTime()
    {
        if (user.exitTime == "" || user.exitTime == null) 
        {
            return;
        }

        TimeSpan datediff = DateTime.Now - DateTime.Parse(user.exitTime);

        int diffDate = datediff.Days * 86400;
        int diffHour = datediff.Hours * 3600;
        int diffMinute = datediff.Minutes * 60;
        int diffSec = diffDate + diffHour + diffMinute + datediff.Seconds;

        if (diffSec < 1200) return;

        BigInteger mPsSum = 0;
        foreach (Staff staff in user.staffs)
        {
            if (staff.level > 0)
            {
                mPsSum += staff.mPs;
            }
        }
        mPsSum /= 10;

        mPsSum *= diffSec;

        if(mPsSum <= 0)
        {
            return;
        }
        Debug.Log(mPsSum);
        Debug.Log(diffSec);
        user.UpdateMoney(mPsSum, true);
        uiManager.ShowRewardPanal(mPsSum);
    }
    public void MoneyPerSecond()
    {
        BigInteger mPsSum = 0;
        foreach (Staff staff in user.staffs)
        {
            mPsSum += staff.mPs;
        }
        if(mPsSum <= 0)
        {
            return; 
        }
        user.money += mPsSum * user.additionMoney;
        uiManager.ShowCoinText(mPsSum * user.additionMoney);
        uiManager.UpdateMoneyPanal();
    }
    public string MoneyUnitConversion(BigInteger value)
    {
        List<int> moneyList = new List<int>();
        int place = (int)Mathf.Pow(10, 4);
        string retStr = "";
        do
        {
            moneyList.Add((int)(value % place));
            value /= place;
        } while (value > 0);


        for (int i = Mathf.Max(0, moneyList.Count - 2); i < moneyList.Count; i++)
        {
            retStr = moneyList[i] + moneyUnit[i] + retStr;
        }

        return retStr;
    }
    public void AutoClick()
    {
        StartCoroutine(AutoClickAnim());
        user.autoClickUsingTime = DateTime.Now.AddSeconds(user.autoClickTime).ToString("G");
        for (int i = 0; i < user.petCount; i++)
        {
            uiManager.OnClickDisPlay();
        }

        uiManager.UpdateMoneyPanal();
    }

    private IEnumerator AutoClickAnim()
    {
        for (int i = 0; i < user.petCount; i++)
        {
            uiManager.ShowClickEffect(new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-4f, 4f), -5f));
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void SettingFrame(int frame)
    {   
        user.frame = frame;
        Application.targetFrameRate = user.frame;
        uiManager.SettingFPS(user.frame);
    }

    public void BGMVolume(float value)
    {
        SoundManager.Inst.BGMVolume(value);
    }

    public void BGMMute(bool isMute)
    {
        SoundManager.Inst.BGMMute(isMute);
    }
    public void EffectMute(bool isMute)
    {
        SoundManager.Inst.EffectMute(isMute);
    }

    public void EffectVolume(float value)
    {
        SoundManager.Inst.EffectVolume(value);
    }
    public void SetBGM(int bgmNum)
    {
        SoundManager.Inst.SetBGM(bgmNum);
    }
    public void SetEffectSound(int effectNum)
    {
        SoundManager.Inst.SetEffectSound(effectNum);
    }



    private void OnApplicationQuit()
    {
        user.exitTime = DateTime.Now.ToString("G");
        Debug.Log("ÀÀ¾Ö");
        SaveToJson();
    }
}
