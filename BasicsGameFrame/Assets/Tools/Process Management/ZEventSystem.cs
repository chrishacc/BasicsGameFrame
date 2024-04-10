using System;
using System.Collections.Generic;

namespace Tool
{
    public interface EventModel { }
    public class ZEventSystem : Singleton<ZEventSystem>
    {
        Dictionary<Type, Action<object>> eventDictionary = new Dictionary<Type, Action<object>>();
        /// <summary>
        /// һ���Զ���(ִ��һ�κ����)
        /// </summary>
        Dictionary<Type, Action<object>> tempEvent = new Dictionary<Type, Action<object>>();
        /// <summary>
        /// �¼�����
        /// </summary>
        /// <typeparam name="T">�¼�</typeparam>
        /// <param name="func">���ĵķ���</param>
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
        /// �¼��ķ���
        /// </summary>
        /// <typeparam name="T">�¼�</typeparam>
        /// <param name="eventData">����</param>
        public void SendEvent<T>(T eventData, bool cleanTempEvent = true) where T : struct, EventModel
        {
            if (eventDictionary.ContainsKey(typeof(T)))
                eventDictionary[typeof(T)].Invoke(eventData);
            if (tempEvent.ContainsKey(typeof(T)))
                tempEvent[typeof(T)].Invoke(eventData);
            if (cleanTempEvent) tempEvent.Clear();
        }
        /// <summary>
        /// �¼��ķ��ͣ��������������null
        /// </summary>
        /// <typeparam name="T">�¼�</typeparam>
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