using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// �ص���ʾ����
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
    /// ����ʵ��
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
    /// �����¼���ָ�����ȼ�
    /// </summary>
    public void Subscribe<T>(string eventName, Action<T> listener, int priority = 0)
    {
        if (!_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] = new List<(Delegate, int)>();
        }
        _eventDictionary[eventName].Add((listener, priority));
        // �����ȼ�����
        _eventDictionary[eventName].Sort((a, b) => b.priority.CompareTo(a.priority));
    }

    /// <summary>
    /// ȡ�������ض��ļ�����
    /// </summary>
    public void Unsubscribe<T>(string eventName, Action<T> listener)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName].RemoveAll(e => e.listener.Equals(listener));
        }
    }

    /// <summary>
    /// �Ƴ����ж�����
    /// </summary>
    public void UnsubscribeAll(string eventName)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName].Clear();
        }
    }

    /// <summary>
    /// �����¼���֧���첽����
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
                    // �����쳣�����Լ�¼��־�ȣ�
                    Console.WriteLine($"Error while handling event '{eventName}': {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// �Զ�����ȡ���뱻���ٶ�����صĶ���
    /// </summary>
    public void CleanUp(object obj)
    {
        foreach (var eventName in _eventDictionary.Keys)
        {
            _eventDictionary[eventName].RemoveAll(e => e.listener.Target == obj);
        }
    }
}
