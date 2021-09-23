using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using BigInteger = System.Numerics.BigInteger;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    public enum EPoolingType { clickEffect, coinText, somSaTang }

    private User user = null;

    private UIManager uiManager = null;

    [SerializeField] private Transform pool = null;

    private Dictionary<EPoolingType, Queue<GameObject>> poolingList = new Dictionary<EPoolingType, Queue<GameObject>>();
    public User CurrentUser { get { return user; } }

    public UIManager UI { get { return uiManager; } }

    public Transform Pool { get { return pool; } }
    public Dictionary<EPoolingType, Queue<GameObject>> PoolingList { get { return poolingList; } }

    private string SAVE_PATH = "";

    private string SAVE_FILENAME = "/SaveFile.txt";

    private string moneyUnits = ",¸¸,¾ï,Á¶,°æ,ÇØ,ÀÀ,¾Ö,±İ,»ç,Çâ,Èï,ÇÏ,ÀÚ,Âï";

    public string[] moneyUnit
    {
        get
        {
            return moneyUnits.Split(',');
        }

    }


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
    private void Awake()
    {
        SAVE_PATH = Application.dataPath + "/Save";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        uiManager = GetComponent<UIManager>();
        MaxPos = new Vector2(2.05f, 4.2f);
        MinPos = new Vector2(-2.05f, -4.2f);
        LoadFromJson();
        SetDict();

    }

    private void Start()
    {
        SettingUser();
        CheckReJoinTime();
        InvokeRepeating("SaveToJson", 5f, 60f);
        InvokeRepeating("AutoClick", 5f, user.autoClickTime);
        InvokeRepeating("MoneyPerSecond", 5f, 1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void SetDict()
    {
        poolingList.Add(EPoolingType.clickEffect, new Queue<GameObject>());
        poolingList.Add(EPoolingType.coinText, new Queue<GameObject>());
        poolingList.Add(EPoolingType.somSaTang, new Queue<GameObject>());
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
            user.userName = "±İ»çÇâ";
            user.basemPc = 10;
            user.additionMoney = 1;
            user.goldCoin = 0;
            user.money = 10000;

            user.staffs.Add(new Staff("ÀÀ¾ÖÂïÂïÀÌ", 0, 0, 0, 1000));
            user.staffs.Add(new Staff("Ã»¼Ò³âÂïÂïÀÌ", 1, 0, 0, 3000));
            user.staffs.Add(new Staff("Áß2º´ÂïÂïÀÌ", 2, 0, 0, 5000));
            user.staffs.Add(new Staff("»õ³»±âÂïÂïÀÌ", 3, 0, 0, 10000));
            user.staffs.Add(new Staff("º¹ÇĞ»ıÂïÂïÀÌ", 4, 0, 0, 15000));
            user.staffs.Add(new Staff("½Å»çÂïÂïÀÌ", 5, 0, 0, 30000));
            user.staffs.Add(new Staff("±â»çÂïÂïÀÌ", 6, 0, 0, 50000));
            user.staffs.Add(new Staff("Áı»çÂïÂïÀÌ", 7, 0, 0, 100000));
            user.staffs.Add(new Staff("¿ÕÂïÂïÀÌ", 8, 0, 0, 300000));
            user.staffs.Add(new Staff("»çÀÌº¸±×ÂïÂïÀÌ", 9, 0, 0, 500000));
            user.staffs.Add(new Staff("AIÂïÂïÀÌ", 10, 0, 0, 1000000));

            user.skills.Add(new Skill("Æ®ÀÌÀ¯", 0, 1, 100, 30, 100));
            user.skills.Add(new Skill("ÀÀ¾Ö", 1, 1, 100, 0, 200));
            user.skills.Add(new Skill("À¯À¸³»¸ğµå", 2, 1, 100, 30, 300));

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

    private void SettingUser()
    {
        if(CheckDate())
        {
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
        foreach (Staff staff in user.staffs)
        {
            user.UpdateMoney(staff.mPs, true);
        }
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
        for (int i = 0; i < user.petAmount; i++)
        {
            user.UpdateMoney(user.mPc, true);
        }
        uiManager.UpdateMoneyPanal();
    }

    private IEnumerator AutoClickAnim()
    {
        for (int i = 0; i < user.petAmount; i++)
        {
            //uiManager.ShowCoinText();
            uiManager.ShowClickEffect(new Vector3(Random.Range(-1.7f, 1.7f), Random.Range(-4f, 4f), -5f));
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void OnApplicationQuit()
    {
        user.exitTime = DateTime.Now.ToString("G");
        SaveToJson();
    }
    private void OnApplicationPause()
    {
        //user.exitTime = DateTime.Now.ToString("G");
        SaveToJson();
    }


}
