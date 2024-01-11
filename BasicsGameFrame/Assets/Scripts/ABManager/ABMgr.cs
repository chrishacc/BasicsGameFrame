using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABMgr : SingletonAutoMono<ABMgr>
{

    private AssetBundle mainAB = null; //主包
    private AssetBundleManifest manifest = null; //依赖包获取用的配置文件

    //AB包不能重复加载
    //字典 用字典来存储 加载过的AB包
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// AB包存放路径 方便修改
    /// </summary>
    private string PathUrl
    {
        get
        {
              return Application.streamingAssetsPath + "/";
        }
    }

    /// <summary>
    /// 主包名 方便修改
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
    /// 加载AB包
    /// </summary>
    /// <param name="abName"></param>
    public void LoadAB(string abName)
    {
        //加载AB包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //获取依赖包相关信息
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);
        for (int i = 0; i < strs.Length; i++)
        {
            //判断包是否加载过
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        //加载资源来源包
        //如果没有加载过 再加载
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDic.Add(abName, ab);
        }
    }

    //同步加载 不指定类型
    public object LoadRes(string abName, string resName)
    {
        LoadAB(abName);

        //加载资源
        return abDic[abName].LoadAsset(resName);
    }

    //同步加载 根据type指定类型
    public object LoadRes(string abName, string resName, System.Type type)
    {
        LoadAB(abName);

        //加载资源
        return abDic[abName].LoadAsset(resName, type);
    }

    //同步加载 根据泛型指定类型
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        LoadAB(abName);

        //加载资源
        return abDic[abName].LoadAsset<T>(resName);
    }

    //异步加载


    //单个包卸载
    public void UnLoad(string abName)
    {
        if(abDic.ContainsKey(abName))
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
        else
        {
            Debug.LogError("要卸载的包不存在");
        }
    }

    //所有包卸载
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
