using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

[System.Serializable]
public class Pool
{
    //public GameObject Prefab { get { return prefab; } }

    public GameObject Prefab => prefab;

    public int Size => size;

    public int RuntimeSize => queue.Count;//运行时池的大小

    [SerializeField] GameObject prefab;

    [SerializeField] int size = 1;//池的大小

    Queue<GameObject> queue;

    Transform parent;

    //对象池初始化
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>(size);
        this.parent = parent;

        for (int i = 0; i < size; i++)
        {

            queue.Enqueue(Copy());
        }
    }

    //对象实例化
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }

    //从对象池取出对象
    GameObject AvailableObject()
    {
        GameObject availableObject = null;
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }

        queue.Enqueue(availableObject);

        return availableObject;
    }

    #region 将取出的对象激活并设置位置、旋转、缩放
    public GameObject PreparedObject()
    {
        GameObject PreparedObject = AvailableObject();
        PreparedObject.SetActive(true);
        return PreparedObject;
    }

    public GameObject PreparedObject(Vector3 position)
    {
        GameObject PreparedObject = AvailableObject();

        PreparedObject.SetActive(true);
        PreparedObject.transform.position = position;

        return PreparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion ratation)
    {
        GameObject PreparedObject = AvailableObject();

        PreparedObject.SetActive(true);
        PreparedObject.transform.position = position;
        PreparedObject.transform.rotation = ratation;

        return PreparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion ratation, Vector3 localScale)
    {
        GameObject PreparedObject = AvailableObject();

        PreparedObject.SetActive(true);
        PreparedObject.transform.position = position;
        PreparedObject.transform.rotation = ratation;
        PreparedObject.transform.localScale = localScale;

        return PreparedObject;
    }
    #endregion
}
