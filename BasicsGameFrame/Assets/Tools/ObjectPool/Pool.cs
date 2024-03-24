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

    public int RuntimeSize => queue.Count;//����ʱ�صĴ�С

    [SerializeField] GameObject prefab;

    [SerializeField] int size = 1;//�صĴ�С

    Queue<GameObject> queue;

    Transform parent;

    //����س�ʼ��
    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>(size);
        this.parent = parent;

        for (int i = 0; i < size; i++)
        {

            queue.Enqueue(Copy());
        }
    }

    //����ʵ����
    GameObject Copy()
    {
        var copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }

    //�Ӷ����ȡ������
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

    #region ��ȡ���Ķ��󼤻����λ�á���ת������
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
