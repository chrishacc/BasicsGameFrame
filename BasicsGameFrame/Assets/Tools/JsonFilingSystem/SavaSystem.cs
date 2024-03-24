using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#region ʹ������
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

    // ����浵ΪJson�ļ�������
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

    // �ӱ��ؼ���Json�ļ�
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

    #region ɾ���浵
    // ɾ���浵�ļ�
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
