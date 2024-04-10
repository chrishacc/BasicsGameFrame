using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
            DontDestroyOnLoad(this.gameObject);
        }

    }

    /// <summary>
    /// 获得单例的状态，为空时返回false
    /// </summary>
    /// <returns>为空时返回false</returns>
    public bool GetTheStateOfInstance()
    {
        return instance != null;
    }

}
