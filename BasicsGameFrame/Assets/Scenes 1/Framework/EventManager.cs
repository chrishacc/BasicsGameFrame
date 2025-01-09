using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// 回调链示例：
/// private void OnPlayerDied(PlayerDiedEvent eventData)
///{
///    Debug.Log("Player died! Event data: " + eventData.Reason);
///    EventManager.Instance.Publish("GameOver", new GameOverEvent());
///}
/// 
/// 
/// 
/// </summary>



public class EventManager
{
    private static EventManager _instance;

    /// <summary>
    /// 单例实例
    /// </summary>
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventManager();
            }
            return _instance;
        }
    }

    private Dictionary<string, List<(Delegate listener, int priority)>> _eventDictionary;

    private EventManager()
    {
        _eventDictionary = new Dictionary<string, List<(Delegate, int)>>();
    }

    /// <summary>
    /// 订阅事件，指定优先级
    /// </summary>
    public void Subscribe<T>(string eventName, Action<T> listener, int priority = 0)
    {
        if (!_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] = new List<(Delegate, int)>();
        }
        _eventDictionary[eventName].Add((listener, priority));
        // 按优先级排序
        _eventDictionary[eventName].Sort((a, b) => b.priority.CompareTo(a.priority));
    }

    /// <summary>
    /// 取消订阅特定的监听器
    /// </summary>
    public void Unsubscribe<T>(string eventName, Action<T> listener)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName].RemoveAll(e => e.listener.Equals(listener));
        }
    }

    /// <summary>
    /// 移除所有订阅者
    /// </summary>
    public void UnsubscribeAll(string eventName)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName].Clear();
        }
    }

    /// <summary>
    /// 发布事件，支持异步处理
    /// </summary>
    public async void Publish<T>(string eventName, T eventData = default)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            foreach (var (listener, _) in _eventDictionary[eventName])
            {
                try
                {
                    if (listener is Action<T> typedListener)
                    {
                        await Task.Run(() => typedListener(eventData));
                    }
                }
                catch (Exception ex)
                {
                    // 处理异常（可以记录日志等）
                    Console.WriteLine($"Error while handling event '{eventName}': {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// 自动清理，取消与被销毁对象相关的订阅
    /// </summary>
    public void CleanUp(object obj)
    {
        foreach (var eventName in _eventDictionary.Keys)
        {
            _eventDictionary[eventName].RemoveAll(e => e.listener.Target == obj);
        }
    }
}
