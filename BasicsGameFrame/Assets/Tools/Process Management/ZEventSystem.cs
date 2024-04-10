using System;
using System.Collections.Generic;

namespace Tool
{
    public interface EventModel { }
    public class ZEventSystem : Singleton<ZEventSystem>
    {
        Dictionary<Type, Action<object>> eventDictionary = new Dictionary<Type, Action<object>>();
        /// <summary>
        /// 一次性订阅(执行一次后清空)
        /// </summary>
        Dictionary<Type, Action<object>> tempEvent = new Dictionary<Type, Action<object>>();
        /// <summary>
        /// 事件订阅
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        /// <param name="func">订阅的方法</param>
        public void SubscribeEvent<T>(Action<T> func, bool isTemp = false) where T : struct, EventModel
        {
            Dictionary<Type, Action<object>> tempDic;
            tempDic = isTemp ? tempEvent : eventDictionary;
            Action<object> action;
            action = (value) =>
            {
                if (value != null)
                    func?.Invoke((T)value);
            };
            if (tempDic.ContainsKey(typeof(T)))
                tempDic[typeof(T)] += action;
            else
                tempDic.Add(typeof(T), action);
        }

        /// <summary>
        /// 事件的发送
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        /// <param name="eventData">参数</param>
        public void SendEvent<T>(T eventData, bool cleanTempEvent = true) where T : struct, EventModel
        {
            if (eventDictionary.ContainsKey(typeof(T)))
                eventDictionary[typeof(T)].Invoke(eventData);
            if (tempEvent.ContainsKey(typeof(T)))
                tempEvent[typeof(T)].Invoke(eventData);
            if (cleanTempEvent) tempEvent.Clear();
        }
        /// <summary>
        /// 事件的发送，不输入参数则传入null
        /// </summary>
        /// <typeparam name="T">事件</typeparam>
        public void SendEvent<T>(bool cleanTempEvent = true) where T : struct, EventModel
        {
            T eventData = new T();
            if (eventDictionary.ContainsKey(typeof(T)))
                eventDictionary[typeof(T)].Invoke(eventData);
            if (tempEvent.ContainsKey(typeof(T)))
                tempEvent[typeof(T)].Invoke(eventData);
            if (cleanTempEvent) tempEvent.Clear();
        }
    }

}