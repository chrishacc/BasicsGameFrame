
using System;
using System.Collections.Generic;
using SpaceFramework.Base;
using UnityEngine;
namespace SpaceFramework.Signal
{
    public interface ISignalManager : IFrameworkComponent<ISignalManager>
    {
        public void SendSignal(string signalName, object signalModel);
        public void SubscribeSignal(string signalName, Action<object> action);
        /// <param name="action">
        /// 应该让注册方自己记录自己会注册的事件，注销时自己注入要注销的事件
        /// 信号管理器不储存任何接收方数据
        /// </param>
        public void UnsubscribeSignal(string signalName, Action<object> action);
    }
    public class SignalManager : ISignalManager
    {
        private Dictionary<string, Action<object>> singDiction;
        private IFrameworkBase frameworkBase;
        public IFrameworkBase GetFramework() => frameworkBase;
        public ISignalManager Init(IFrameworkBase frameworkBase)
        {
            this.frameworkBase = frameworkBase;
            Init();
            return this;
        }
        public ISignalManager Init()
        {
            singDiction = new Dictionary<string, Action<object>>();
            return this;
        }

        public void SendSignal(string signalName, object signalModel)
        {
            if (singDiction.ContainsKey(signalName))
                singDiction[signalName]?.Invoke(signalModel);
        }
        public void SubscribeSignal(string signalName, Action<object> action)
        {
            if (singDiction.ContainsKey(signalName))
                singDiction[signalName] += action;
            else
            {
                Action<object> newAction = action;
                singDiction.Add(signalName, newAction);
            }
        }

        public void UnsubscribeSignal(string signalName, Action<object> action)
        {
            if (singDiction.ContainsKey(signalName))
                singDiction[signalName] -= action;
        }
    }
    public class SignalBehavior : AFrameworkComponent<SignalBehavior>
    {
        public ICouldSendSignal sendSignalComponent;
        public ICouldSubscribeSignal subscribeSignalComponent;
        public override SignalBehavior Init()
        {
            sendSignalComponent = new CouldSendSignalComponent().Init(GetFramework());
            subscribeSignalComponent = new CouldSubscribeSignalComponent().Init(GetFramework());
            return this;
        }
        public void SubscribeSignal<T>(string signalName, Action<T> action)
        {
            Action<object> newAction = (e) => { action.Invoke((T)e); };
            subscribeSignalComponent.SubscribeSignal(signalName, newAction);
        }
        public void UnsubscribeSignal(string signalName)
        { subscribeSignalComponent.UnsubscribeSignal(signalName); }
        public void SendSignal(string signalName, object singModel)
        { sendSignalComponent.SendSignal(signalName, singModel); }
    }
    /// <summary>
    /// 抽象的接受信号的组件
    /// </summary>
    public class CouldSendSignalComponent : AFrameworkComponent<ICouldSendSignal>, ICouldSendSignal
    {
        public override ICouldSendSignal Init()
        {
            return this;
        }
        public void SendSingal<T>(string signalName, T singModel)
        {
            SendSignal(signalName, singModel);
        }
        public void SendSignal(string signalName, object singModel)
        {
            GetFramework().SignalManager.SendSignal(signalName, singModel);
        }
    }
    /// <summary>
    /// 抽象的可发信号的行为组件
    /// 继承后重写对应的框架接收器
    /// </summary>
    public class CouldSubscribeSignalComponent : AFrameworkComponent<ICouldSubscribeSignal>, ICouldSubscribeSignal
    {
        public Dictionary<string, Action<object>> selfActionDiction = new Dictionary<string, Action<object>>();
        public Action<object> GetSelfSignal(string signalName)
        {
            if (selfActionDiction.ContainsKey(signalName))
                return selfActionDiction[signalName];
            return null;
        }
        public override ICouldSubscribeSignal Init()
        {
            return this;
        }

        public void SubscribeSignal<T>(string signalName, Action<T> action)
        {
            SubscribeSignal(signalName, action);
        }
        public void SubscribeSignal(string signalName, Action<object> action)
        {
            if (selfActionDiction.ContainsKey(signalName))
                UnsubscribeSignal(signalName);
            selfActionDiction.Add(signalName, action);
            GetFramework().SignalManager.SubscribeSignal(signalName, selfActionDiction[signalName]);
        }
        public void UnsubscribeSignal(string signalName)
        {
            GetFramework().SignalManager.UnsubscribeSignal(signalName, GetSelfSignal(signalName));
            selfActionDiction.Remove(signalName);
        }
    }
    public interface ICouldSendSignal : IFrameworkComponent<ICouldSendSignal>
    {
        public void SendSignal(string signalName, object singModel);
    }
    public interface ICouldSubscribeSignal : IFrameworkComponent<ICouldSubscribeSignal>
    {
        /// <summary>
        /// 根据名字找到对应的自身的事件用来删除
        /// 因为删除时自己传入自己订阅过的事件而不由管理器存储
        /// </summary>
        /// <param name="signalName"></param>
        public Action<object> GetSelfSignal(string signalName);
        public void SubscribeSignal(string signalName, Action<object> action);
        /// <summary>
        /// 具体的Action从字典里找
        /// </summary>
        /// <param name="signalName"></param>
        public void UnsubscribeSignal(string signalName);
    }
}