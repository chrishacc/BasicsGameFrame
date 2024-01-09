using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载式 继承MonoBehaviour的单例模式基类
/// 不建议使用，容易破坏单例模式的唯一性
/// 1、挂载多个脚本
/// 2、切换场景回来时，由于MonoBehaviour的生命周期，会重新调用Awake方法，就会又多一个该单例模式对象，导致单例模式的唯一性被破坏
/// 3、还可以通过代码动态创建对象，也会破坏单例模式的唯一性
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


//使用示例
//public class Test : SingletonMono<Test>
//{
//    protected override void Awake()
//    {
//        //重写Awake方法时 ，必须调用base.Awake()方法
//        base.Awake();
//        Debug.Log("Test Awake");
//    }


//}