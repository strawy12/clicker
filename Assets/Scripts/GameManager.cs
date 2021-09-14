using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameManager : MonoSingleton<GameManager>
{
     private User user = null;

    public UIManager uiManager { get; private set; }
    public User CurrentUser { get { return user; } }

    private string SAVE_PATH = "";

    private string SAVE_FILENAME = "/SaveFile.txt";

    public Vector2 MaxPos { get; private set; } = new Vector2(1.5f, 3f);
    public Vector2 MinPos { get; private set; } = new Vector2(-1.25f, 0);


    

    private int cnt = 3;
    private void Awake()
    {
        SAVE_PATH = Application.dataPath + "/Save";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        LoadFromJson();
        uiManager = GetComponent<UIManager>();
        
        InvokeRepeating("SaveToJson", 1f, 60f);
        InvokeRepeating("AutoClick", 1f, 5f);
        //InvokeRepeating("MoneyPerSecond", 0f, 1f);
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
            user.userName = "�ݻ���";
            user.basezPc = 10;
            user.money = 10000;
            user.maxPeople = 5;
            user.peopleCnt = 0;
            user.mileage = 0;

            user.staffs.Add(new Staff("����������", 0, 0, 0, 1000, 500, 500));
            user.staffs.Add(new Staff("û�ҳ�������", 1, 0, 0, 3000, 700, 132));
            user.staffs.Add(new Staff("��2��������", 2, 0, 0, 5000, 1000, 110));
            user.staffs.Add(new Staff("������������", 3, 0, 0, 10000, 1250, 70)); 
            user.staffs.Add(new Staff("���л�������", 4, 0, 0, 15000, 1500, 60)); 
            user.staffs.Add(new Staff("�Ż�������", 5, 0, 0, 30000, 1750, 48));
            user.staffs.Add(new Staff("���������", 6, 0, 0, 50000, 2000, 32));
            user.staffs.Add(new Staff("����������", 7, 0, 0, 100000, 2500, 20));
            user.staffs.Add(new Staff("��������", 8, 0, 0, 300000, 3000, 15));
            user.staffs.Add(new Staff("���̺���������", 9, 0, 0, 500000, 5000, 7)); 
            user.staffs.Add(new Staff("AI������", 10, 0, 0, 1000000, 10000, 5)); 
            user.staffs.Add(new Staff("���", 11, 0, 0, 10000000, 100000, 1));
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
        foreach(Staff soldier in user.staffs)
        {
            user.money += soldier.amount * soldier.mPs;
        }
        uiManager.UpdateMoneyPanal();
    }

    public void AutoClick()
    {
        StartCoroutine(AutoClickAnim());
        for(int i = 0; i < user.peopleCnt; i++)
        {
            user.money += user.zpc;
        }
        uiManager.UpdateMoneyPanal();
    }

    private IEnumerator AutoClickAnim()
    {
        for (int i = 0; i < user.peopleCnt; i++)
        {
            uiManager.SpawnClickText(user.zpc);
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}
