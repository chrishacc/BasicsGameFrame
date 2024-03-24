using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// ����ģʽ���� ��ҪĿ���Ǳ����������� ����ʵ�ֵ���ģʽ����
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonBaseClass<T> where T : class, new()
{
    private static T instance;

    //ͨ�����Եķ�ʽ��ȡ����   
    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                //instance = new T();
                //���÷���õ��޲�˽�еĹ��캯�� �����ڶ����ʾ���� PS�����������ܿ���
                Type type = typeof(T);
                ConstructorInfo info = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    Type.EmptyTypes,
                    null);
                if(info != null)
                {
                    instance = info.Invoke(null) as T;
                }
                else
                {
                    Debug.LogError("û�еõ���Ӧ���޲�˽�й��캯��");
                }
            }
            return instance;
        }
    }

    //ͨ�������ķ�ʽ��ȡ����
    //public static T GetInstance()
    //{
    //    if (instance == null)
    //    {
    //        instance = new T();
    //    }
    //    return instance;
    //}
}

