using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ʽ �̳�MonoBehaviour�ĵ���ģʽ����
/// ������ʹ�ã������ƻ�����ģʽ��Ψһ��
/// 1�����ض���ű�
/// 2���л���������ʱ������MonoBehaviour���������ڣ������µ���Awake�������ͻ��ֶ�һ���õ���ģʽ���󣬵��µ���ģʽ��Ψһ�Ա��ƻ�
/// 3��������ͨ�����붯̬��������Ҳ���ƻ�����ģʽ��Ψһ��
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}


//ʹ��ʾ��
//public class Test : SingletonMono<Test>
//{
//    protected override void Awake()
//    {
//        //��дAwake����ʱ ���������base.Awake()����
//        base.Awake();
//        Debug.Log("Test Awake");
//    }


//}