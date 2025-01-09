using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Instance.Subscribe<PlayerDiedEvent>("PlayerDied", OnPlayerDied);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe<PlayerDiedEvent>("PlayerDied", OnPlayerDied);
    }

    private void OnPlayerDied(PlayerDiedEvent eventData)
    {
        Debug.Log("Player died! Event data: " + eventData.Reason);
    }
}

// 事件数据类
public class PlayerDiedEvent
{
    public string Reason { get; set; }
    public PlayerDiedEvent(string reason) { Reason = reason; }

}
