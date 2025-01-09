using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTest : MonoBehaviour
{
    private void Awake()
    {
        // 确保 EventManager 实例被创建
        var eventManager = EventManager.Instance;
    }

    private void Start()
    {
        // 订阅事件
        //EventManager.Instance.Subscribe("TestEvent", OnTestEvent);
        // 发布事件
        //EventManager.Instance.Publish("TestEvent", "Hello, World!");
        PlayerDeath();
    }

    public void PlayerDeath()
    {
        var eventData = new PlayerDiedEvent("Fell off the map");
        EventManager.Instance.Publish("PlayerDied", eventData);
    }
}
