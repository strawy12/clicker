using System.IO;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private User user = null;
    
    public User CurrentUser { get { return user; } }

    private string SAVE_PATH = "";

    private string SAVE_FILENAME = "/SaveFile.txt";

    private UIManager uiManager => GetComponent<UIManager>();
    public UIManager UI { get { return uiManager; } }
    private void Awake()
    {
        SAVE_PATH = Application.dataPath + "/Save";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }
        LoadFromJson();
        InvokeRepeating("SaveToJson", 1f, 60f);
        InvokeRepeating("EarnEnergyPerSencond", 0f, 1f);
    }

    private void LoadFromJson()
    {
        string json = "";
        if(File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            user = JsonUtility.FromJson<User>(json); 
        }
    }

    private void SaveToJson()
    {
        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
    }

    public void EarnEnergyPerSencond()
    {
        foreach(Soldier soldier in user.soldiers)
        {
            user.energy += soldier.ePs * soldier.amount;
        }
        UI.UpdateEnergyPanal();
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }
}
