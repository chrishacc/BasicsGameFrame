using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ABMgr : SingletonAutoMono<ABMgr>
{

    private AssetBundle mainAB = null; //����
    private AssetBundleManifest manifest = null; //��������ȡ�õ������ļ�

    //AB�������ظ�����
    //�ֵ� ���ֵ����洢 ���ع���AB��
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// AB�����·�� �����޸�
    /// </summary>
    private string PathUrl
    {
        get
        {
              return Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// ������ �����޸�
    /// </summary>
    private string MainABName
    {
        get
        {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
            return "PC";
            #endif
        }
    }

    /// <summary>
    /// ����AB��
    /// </summary>
    /// <param name="abName"></param>
    public void LoadAB(string abName)
    {
        //����AB��
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //��ȡ�����������Ϣ
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //�жϰ��Ƿ���ع�
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        //������Դ��Դ��
        //���û�м��ع� �ټ���
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    //ͬ������ ��ָ������
    public object LoadRes(string abName, string resName)
    {
        LoadAB(abName);

        //������Դ
        return abDic[abName].LoadAsset(resName);
    }

    //ͬ������ ����typeָ������
    public object LoadRes(string abName, string resName, System.Type type)
    {
        LoadAB(abName);

        //������Դ
        return abDic[abName].LoadAsset(resName, type);
    }

    //ͬ������ ���ݷ���ָ������
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        LoadAB(abName);

        //������Դ
        return abDic[abName].LoadAsset<T>(resName);
    }

    //�첽����
    //������첽���� AB����û��ʹ���첽����
    //ֻ�Ǵ�AB���� ������Դʱ ʹ���첽

    //�������� �첽������Դ
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        LoadAB(abName);

        //������Դ
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;

        //�첽���ؽ����� ͨ��ί�� ���ݸ��ⲿ �ⲿ��ʹ��
        if(abr.asset is GameObject)
        {
            callBack(abr.asset);
        }
        else
        {
            Debug.LogError("��Դ����ʧ��");
        }

    }

    //����type �첽������Դ
    public void LoadResAsync(string abName, string resName, System.Type type,UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, type,callBack));
    }

    private IEnumerator ReallyLoadResAsync(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        LoadAB(abName);

        //������Դ
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName, type);
        yield return abr;

        //�첽���ؽ����� ͨ��ί�� ���ݸ��ⲿ �ⲿ��ʹ��
        if (abr.asset is GameObject)
        {
            callBack(abr.asset);
        }
        else
        {
            Debug.LogError("��Դ����ʧ��");
        }

    }

    //���ݷ��� �첽������Դ
    public void LoadResAsync<T>(string abName, string resName, UnityAction<Object> callBack) where T : Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
    }

    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<Object> callBack) where T : Object
    {
        LoadAB(abName);

        //������Դ
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;

        //�첽���ؽ����� ͨ��ί�� ���ݸ��ⲿ �ⲿ��ʹ��
        if (abr.asset is GameObject)
        {
            callBack(abr.asset);
        }
        else
        {
            Debug.LogError("��Դ����ʧ��");
        }

    }

    //������ж��
    public void UnLoad(string abName)
    {
        if(abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
        else
        {
            Debug.LogError("Ҫж�صİ�������");
        }
    }

    //���а�ж��
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
