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
            user.userName = "±İ»çÇâ";
            user.basemPc = 10;
            user.maxPeople = 5;
            user.peopleCnt = 0;

            user.soldiers.Add(new Soldier("ÀÀ¾ÖÂïÂïÀÌ", 0, 0, 0, 1000, 500, 500));
            user.soldiers.Add(new Soldier("Ã»¼Ò³âÂïÂïÀÌ", 1, 0, 0, 3000, 700, 85));
            user.soldiers.Add(new Soldier("Áß2º´ÂïÂïÀÌ", 2, 0, 0, 5000, 1000, 85));
            user.soldiers.Add(new Soldier("»õ³»±âÂïÂïÀÌ", 3, 0, 0, 10000, 1250, 85));
            user.soldiers.Add(new Soldier("º¹ÇĞ»ıÂïÂïÀÌ", 4, 0, 0, 15000, 1500, 85));
            user.soldiers.Add(new Soldier("½Å»çÂïÂïÀÌ", 5, 0, 0, 30000, 1750, 35));
            user.soldiers.Add(new Soldier("±â»çÂïÂïÀÌ", 6, 0, 0, 50000, 2000, 35));
            user.soldiers.Add(new Soldier("Áı»çÂïÂïÀÌ", 7, 0, 0, 100000, 2500, 35));
            user.soldiers.Add(new Soldier("¿ÕÂïÂïÀÌ", 8, 0, 0, 300000, 3000, 35));
            user.soldiers.Add(new Soldier("»çÀÌº¸±×ÂïÂïÀÌ", 9, 0, 0, 500000, 5000, 9));
            user.soldiers.Add(new Soldier("AIÂïÂïÀÌ", 10, 0, 0, 1000000, 10000, 9));
            user.soldiers.Add(new Soldier("±î¹Ì", 11, 0, 0, 10000000, 100000, 2));
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
        foreach(Soldier soldier in user.soldiers)
        {
            user.money += soldier.amount * soldier.mPs;
        }
        uiManager.UpdateEnergyPanal();
    }

    public void AutoClick()
    {
        StartCoroutine(AutoClickAnim());
        for(int i = 0; i < user.peopleCnt; i++)
        {
            user.money += user.mpc;
        }
        uiManager.UpdateEnergyPanal();
    }

    private IEnumerator AutoClickAnim()
    {
        for (int i = 0; i < user.peopleCnt; i++)
        {
            uiManager.SpawnClickText(user.mpc);
            yield return new WaitForSeconds(0.05f);
        }
    }
    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}
