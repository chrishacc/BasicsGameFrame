using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTest : MonoBehaviour
{
    private void Awake()
    {
        // ȷ�� EventManager ʵ��������
        var eventManager = EventManager.Instance;
    }

    private void Start()
    {
        // �����¼�
        //EventManager.Instance.Subscribe("TestEvent", OnTestEvent);
        // �����¼�
        //EventManager.Instance.Publish("TestEvent", "Hello, World!");
        PlayerDeath();
    }

    public void PlayerDeath()
    {
        var eventData = new PlayerDiedEvent("Fell off the map");
        EventManager.Instance.Publish("PlayerDied", eventData);
    }
}
