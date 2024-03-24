using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 单例模式基类 主要目的是避免代码的冗余 方便实现单例模式的类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonBaseClass<T> where T : class, new()
{
    private static T instance;

    //通过属性的方式获取单例   
    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                //instance = new T();
                //利用反射得到无参私有的构造函数 来用于对象的示例化 PS：反射有性能开销
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
                    Debug.LogError("没有得到对应的无参私有构造函数");
                }
            }
            return instance;
        }
    }

    //通过方法的方式获取单例
    //public static T GetInstance()
    //{
    //    if (instance == null)
    //    {
    //        instance = new T();
    //    }
    //    return instance;
    //}
}

