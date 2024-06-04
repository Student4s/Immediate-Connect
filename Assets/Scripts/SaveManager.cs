using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    public string timeStamp;
}

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;
    private string saveFilePath;
    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("SaveManager").AddComponent<SaveManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    public void SaveTime()
    {
        SaveData data = new SaveData
        {
            timeStamp = DateTime.Now.ToString("o")
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
    }

    public DateTime? LoadTime()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            if (DateTime.TryParse(data.timeStamp, out DateTime savedTime))
            {
                return savedTime;
            }
        }
        return null;
    }
}