using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
