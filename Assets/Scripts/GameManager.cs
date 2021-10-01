using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using BigInteger = System.Numerics.BigInteger;
using Random = UnityEngine.Random;
public class ScreenSize
{
    public static float GetScreenToWorldHeight
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var height = edgeVector.y * 0.5f;
            return height;
        }
    }
    public static float GetScreenToWorldWidth
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVector.x * 0.5f;
            return width;
        }
    }
}

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

    private string SAVE_FILENAME = "/SaveFile.txt";

    private string moneyUnits = ",∏∏,æÔ,¡∂,∞Ê,«ÿ,¿¿,æ÷,±›,ªÁ,«‚,»Ô,«œ,¿⁄,¬Ô";

    public string[] moneyUnit
    {
        get
        {
            return moneyUnits.Split(',');
        }

    }

    private float timer = 0f;


    public Vector2 MaxPos { get; private set; }
    public Vector2 MinPos { get; private set; }
    public Vector3 MousePos
    {
        get
        {
            Vector3 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            result.x = Mathf.Clamp(result.x, MinPos.x, MaxPos.x);
            result.y = Mathf.Clamp(result.y, MinPos.y, MaxPos.y);
            result.z = -10;
            return result;
        }
    }

    private int cnt = 3;
    public void Awake()
    {
        SAVE_PATH = Application.persistentDataPath + "/Save";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        MaxPos = new Vector2(2.05f, 4.2f);
        MinPos = new Vector2(-2.05f, -4.2f);
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
            Application.Quit();
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

    public void OnClickDeleteJson()
    {
        uiManager.ShowMessage("µ•¿Ã≈Õ∏¶ ªË¡¶«’¥œ¥Ÿ. √ ±‚»≠∏¶ ¿ß«ÿ ∞‘¿”¿ª ¡æ∑·«’¥œ¥Ÿ.", 0.3f, 0.1f, 1.5f, 22);
        Invoke("DeleteJson", 1.5f);
    }

    public void DeleteJson()
    {
        File.Delete(SAVE_PATH + SAVE_FILENAME);
        user = null;
        Application.Quit();
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
            user.userName = "±›ªÁ«‚";
            user.basemPc = 10;
            user.additionMoney = 1;
            user.frame = 60;
            user.goldCoin = 0;
            user.money = 100;
            user.sahayang = new Sahayang();
            user.isTuto = new bool[5];
            user.missionsClear = new bool[6];
            user.sahayang.price = 1000;
            user.bgmVolume = 0.5f;
            user.effectVolume = 0.5f;

            user.staffs.Add(new Staff("¿¿æ÷¬Ô¬Ô¿Ã", 0, 0, 10, 1000));
            user.staffs.Add(new Staff("º“≥‚¬Ô¬Ô¿Ã", 1, 0, 200, 2000));
            user.staffs.Add(new Staff("º“≥‡¬Ô¬Ô¿Ã", 2, 0, 3000, 30000));
            user.staffs.Add(new Staff("æÀπŸ¬Ô¬Ô¿Ã", 3, 0, 20000, 200000));
            user.staffs.Add(new Staff("≥Û∫Œ¬Ô¬Ô¿Ã", 4, 0, 150000, 1500000));
            user.staffs.Add(new Staff("∫πº≠¬Ô¬Ô¿Ã", 5, 0, 1000000, 10000000));
            user.staffs.Add(new Staff("¿«ªÁ¬Ô¬Ô¿Ã", 6, 0, 7000000, 70000000));
            user.staffs.Add(new Staff("º“πÊ∞¸¬Ô¬Ô¿Ã", 7, 0, 15000000, 150000000));
            user.staffs.Add(new Staff("«Ô√¢¬Ô¬Ô¿Ã", 8, 0, 30000000, 300000000));
            user.staffs.Add(new Staff("±∫¿Œ¬Ô¬Ô¿Ã", 9, 0, 50000000, 500000000));
            user.staffs.Add(new Staff("OIF¬Ô¬Ô¿Ã", 10, 0, 100000000, 1000000000));

            user.skills.Add(new Skill("¬¯√Î¿« «ˆ¿Â", 0, 1, 100, 30, 100));
            user.skills.Add(new Skill("≥Îµø¿« ¥Î∞°", 1, 1, 100, 0, 200));
            user.skills.Add(new Skill("¿Ø¿∏≥ª ∏µÂ", 2, 1, 100, 30, 300));

            user.pets.Add(new Pet(0, "∞≠æ∆¡ˆ", 0, 0, 20, 1000));
            user.pets.Add(new Pet(1, "≈‰≥¢", 0, 0, 20, 1000));
            user.pets.Add(new Pet(2, "ø©øÏ", 0, 0, 15, 1000));
            user.pets.Add(new Pet(3, "∞ı", 0, 0, 10, 1000));
            user.pets.Add(new Pet(4, "∆Î±œ", 0, 0, 10, 1000));
            user.pets.Add(new Pet(5, "≥ ±∏∏Æ", 0, 0, 7, 1000));
            user.pets.Add(new Pet(6, "µ≈¡ˆ", 0, 0, 7, 1000));
            user.pets.Add(new Pet(7, "æÁ", 0, 0, 5, 1000));
            user.pets.Add(new Pet(8, "¥Ÿ∂˜¡„", 0, 0, 5, 1000));
            user.pets.Add(new Pet(9, "±ÓπÃ", 0, 0, 1, 1000));

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
        if (user.exitTime == null) return;
        TimeSpan datediff = DateTime.Now - DateTime.Parse(user.exitTime);
        if (datediff.Minutes < 20) return;

        int diffSec = datediff.Seconds;
        BigInteger mPsSum = 0;
        foreach (Staff staff in user.staffs)
        {
            if (staff.level > 0)
            {
                mPsSum += staff.mPs;
            }
        }

        mPsSum /= 10;
        user.UpdateMoney(mPsSum * diffSec, true);
        uiManager.ShowRewardPanal(mPsSum * diffSec);
    }
    public void MoneyPerSecond()
    {
        BigInteger mPsSum = 0;
        foreach (Staff staff in user.staffs)
        {
            mPsSum += staff.mPs;
        }
        user.money += mPsSum * user.additionMoney;
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
            user.UpdateMoney(user.mPc, true);
        }
        uiManager.UpdateMoneyPanal();
    }

    private IEnumerator AutoClickAnim()
    {
        for (int i = 0; i < user.petCount; i++)
        {
            //uiManager.ShowCoinText();
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
    private void OnApplicationQuit()
    {
        user.exitTime = DateTime.Now.ToString("G");
        SaveToJson();
    }
    private void OnApplicationPause()
    {
        user.exitTime = DateTime.Now.ToString("G");
        SaveToJson();


    }


}
