using System.IO;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private User user = null;

    public UIManager uiManager { get; private set; }
    public User CurrentUser { get { return user; } }

    private string SAVE_PATH = "";

    private string SAVE_FILENAME = "/SaveFile.txt";

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
        else
        {
            user.userName = "�ݻ���";
            user.mPc = 10;
            user.maxPeople = 5;
            user.peopleCnt = 0;
            user.soldiers.Add(new Soldier("����������", 0, 0, 1000, 500));
            user.soldiers.Add(new Soldier("û�ҳ�������", 0, 0, 3000, 700));
            user.soldiers.Add(new Soldier("��2��������", 0, 0, 5000, 1000));
            user.soldiers.Add(new Soldier("������������", 0, 0, 10000, 1250));
            user.soldiers.Add(new Soldier("���л�������", 0, 0, 15000, 1500));
            user.soldiers.Add(new Soldier("�Ż�������", 0, 0, 30000, 1750));
            user.soldiers.Add(new Soldier("���������", 0, 0, 50000, 2000));
            user.soldiers.Add(new Soldier("����������", 0, 0, 100000, 2500));
            user.soldiers.Add(new Soldier("��������", 0, 0, 300000, 3000));
            user.soldiers.Add(new Soldier("���̺���������", 0, 0, 500000, 5000));
            user.soldiers.Add(new Soldier("AI������", 0, 0, 1000000, 10000));
            user.soldiers.Add(new Soldier("���", 0, 0, 10000000, 100000));
        }
    }

    private void SaveToJson()
    {
        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
    }

    public void MoneyPerSecond()
    {
        foreach(Soldier soldier in user.soldiers)
        {
            user.money += soldier.amount * soldier.mPs;
        }
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}
