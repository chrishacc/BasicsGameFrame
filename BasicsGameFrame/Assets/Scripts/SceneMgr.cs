using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// �����׳���������
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
            //ͨ��Lambda���ʽ ����һ��
            //���ڲ� ֱ�ӵ����ⲿ�����ί�м���
            action?.Invoke();
        };
    }
}

//ʹ��ʾ��
//SceneMgr.Instance.LoadScene("Game", () => {
//    Debug.Log("������ɺ���߼�");
//});
