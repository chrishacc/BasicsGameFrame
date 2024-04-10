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
    /// ��õ�����״̬��Ϊ��ʱ����false
    /// </summary>
    /// <returns>Ϊ��ʱ����false</returns>
    public bool GetTheStateOfInstance()
    {
        return instance != null;
    }

}
