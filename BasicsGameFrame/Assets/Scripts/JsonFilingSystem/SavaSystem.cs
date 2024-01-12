using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#region 使用用例
//const string saveFileName = "SaveData.json";

//void SavaByJsonExample()
//{
//    var data = new SaveData();

//    SaveSystem.SaveByJson(saveFileName, data);
//}

//void LoadFromJsonExample()
//{
//    var data = SaveSystem.LoadByJson<SaveData>(saveFileName);

//    LoadDataExample(data);
//}

//public static void DeleteDataSaveFileExample()
//{
//    SaveSystem.DeleteSaveFile(saveFileName);
//}

#endregion

public static class SaveSystem
{
    #region JsonFilingSystem

    // 保存存档为Json文件到本地
    public static void SaveByJson(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.WriteAllText(path, json);

            Debug.Log($"Susscessfully Saved data to  + {path}.");
        }
        catch (System.Exception exception)
        {
            Debug.Log($"Failed to save data to + {path}.\n{exception}");
        }

    }

    // 从本地加载Json文件
    public static T LoadByJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);

            Debug.Log($"Susscessfully Loaded data from  + {path}.");

            return data;
        }
        catch (System.Exception exception)
        {
            Debug.Log($"Failed to load data from + {path}.\n{exception}");

            return default;
        }
    }

    #endregion

    #region 删除存档
    // 删除存档文件
    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.Delete(path);

            Debug.Log($"Susscessfully Deleted data from  + {path}.");
        }
        catch (System.Exception exception)
        {
            Debug.Log($"Failed to delete data from + {path}.\n{exception}");
        }
    }

    #endregion

}
