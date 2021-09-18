using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public enum EPanalState { staff, company, level };
    public enum EPoolingType { clickEffect, coinText }

    private User user = null;

    private UIManager uiManager = null;

    [SerializeField] private Transform pool = null;

    private Dictionary<EPoolingType, Queue<GameObject>> poolingList = new Dictionary<EPoolingType, Queue<GameObject>>();
    public User CurrentUser { get { return user; } }

    public UIManager UI { get { return uiManager; } }

    public Transform Pool { get {return pool; } } 
    public Dictionary<EPoolingType, Queue<GameObject>> PoolingList { get {return poolingList; } } 

    private string SAVE_PATH = "";

    private string SAVE_FILENAME = "/SaveFile.txt";

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
        LoadFromJson();
        SetDict();
        uiManager = GetComponent<UIManager>();
        MaxPos = new Vector2(4.1f, 1.7f);
        MinPos = new Vector2(-3.6f, -1.7f);
        InvokeRepeating("SaveToJson", 1f, 60f);
       // InvokeRepeating("AutoClick", 1f, 5f);
        //InvokeRepeating("MoneyPerSecond", 0f, 1f);
    }

    private void SetDict()
    {
        poolingList.Add(EPoolingType.clickEffect, new Queue<GameObject>());
        poolingList.Add(EPoolingType.coinText, new Queue<GameObject>());
    }


    private void LoadFromJson()
    {
        string json = "";
        if(File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            user = JsonUtility.FromJson<User>(json); 
        }
        if (user == null)
        {
            user = new User();
            user.userName = "±İ»çÇâ";
            user.basemPc = 10;
            user.money = 10000;
            user.maxPeople = 5;
            user.mileage = 0;

            user.staffs.Add(new Staff("ÀÀ¾ÖÂïÂïÀÌ", 0, 0, 0, 1000, 500));
            user.staffs.Add(new Staff("Ã»¼Ò³âÂïÂïÀÌ", 1, 0, 0, 3000, 700));
            user.staffs.Add(new Staff("Áß2º´ÂïÂïÀÌ", 2, 0, 0, 5000, 1000));
            user.staffs.Add(new Staff("»õ³»±âÂïÂïÀÌ", 3, 0, 0, 10000, 1250)); 
            user.staffs.Add(new Staff("º¹ÇĞ»ıÂïÂïÀÌ", 4, 0, 0, 15000, 1500)); 
            user.staffs.Add(new Staff("½Å»çÂïÂïÀÌ", 5, 0, 0, 30000, 1750));
            user.staffs.Add(new Staff("±â»çÂïÂïÀÌ", 6, 0, 0, 50000, 2000));
            user.staffs.Add(new Staff("Áı»çÂïÂïÀÌ", 7, 0, 0, 100000, 2500));
            user.staffs.Add(new Staff("¿ÕÂïÂïÀÌ", 8, 0, 0, 300000, 3000));
            user.staffs.Add(new Staff("»çÀÌº¸±×ÂïÂïÀÌ", 9, 0, 0, 500000, 5000)); 
            user.staffs.Add(new Staff("AIÂïÂïÀÌ", 10, 0, 0, 1000000, 10000)); 

            user.skills.Add(new Skill("Æ®ÀÌÀ¯", 0, 0, 1000));
            user.skills.Add(new Skill("ÀÀ¾Ö", 1, 0, 2000));
            user.skills.Add(new Skill("À¯À¸³»¸ğµå", 2, 0, 3000));

            user.pets.Add(new Pet(0, "°­¾ÆÁö", 0, 0, 1000));
            user.pets.Add(new Pet(1, "Åä³¢", 0, 0, 2000));
            user.pets.Add(new Pet(2, "¿©¿ì", 0, 0, 3000));
            user.pets.Add(new Pet(3, "°õ", 0, 0, 4000));
            user.pets.Add(new Pet(4, "Æë±Ï", 0, 0, 5000));
            user.pets.Add(new Pet(5, "³Ê±¸¸®", 0, 0, 6000));
            user.pets.Add(new Pet(6, "µÅÁö", 0, 0, 7000));
            user.pets.Add(new Pet(7, "¾ç", 0, 0, 8000));
            user.pets.Add(new Pet(8, "´Ù¶÷Áã", 0, 0, 9000));
            user.pets.Add(new Pet(9, "±î¹Ì", 0, 0, 10000));

            SaveToJson();
        }
    }

    private void SaveToJson()
    {
        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
    }

    public void MoneyPerSecond()
    {
        foreach(Staff staff in user.staffs)
        {
            user.money += staff.amount * staff.mPs;
        }
        uiManager.UpdateMoneyPanal();
    }

    public void AutoClick()
    {
        StartCoroutine(AutoClickAnim());
        for(int i = 0; i < user.peopleCnt; i++)
        {
            user.money += user.mPc;
        }
        uiManager.UpdateMoneyPanal();
    }

    private IEnumerator AutoClickAnim()
    {
        for (int i = 0; i < user.peopleCnt; i++)
        {
            //uiManager.ShowCoinText();
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}
