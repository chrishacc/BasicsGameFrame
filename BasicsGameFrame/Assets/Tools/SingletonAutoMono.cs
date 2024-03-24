using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Զ�����ʽ�� �̳�MonoBehaviour�ĵ���ģʽ����
/// �Ƽ�ʹ��
/// �����ֶ����� ���趯̬��� ��������г�������������
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            //��̬���� ��̬����
            //�ڳ����ϴ���������
            GameObject obj = new GameObject();
            //�õ�T�ű������� Ϊ������� �����ڱ༭���п�����ȷ�Ŀ����õ���ģʽ�ű�����������GameObject
            obj.name = typeof(T).ToString();
            //��̬���ض�Ӧ�� ����ģʽ�ű�
            instance = obj.AddComponent<T>();
            //������ʱ���Ƴ����� ��֤����������Ϸ���������ж�����
            DontDestroyOnLoad(obj);
            return instance;
        }
    }

}