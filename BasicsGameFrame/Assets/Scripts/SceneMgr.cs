using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 超简易场景管理单例
/// </summary>
public class SceneMgr
{
    private static SceneMgr instance = new SceneMgr();

    public static SceneMgr Instance => instance;

    private SceneMgr()
    {
        Debug.Log("SceneMgr Constructor");
    }

    public void LoadScene(string sceneName, UnityAction action)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.completed += (a) =>
        {
            //通过Lambda表达式 包裹一层
            //在内部 直接调用外部传入的委托即可
            action?.Invoke();
        };
    }
}

//使用示例
//SceneMgr.Instance.LoadScene("Game", () => {
//    Debug.Log("加载完成后的逻辑");
//});
