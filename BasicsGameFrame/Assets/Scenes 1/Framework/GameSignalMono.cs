using System;
using SpaceFramework.Base;
using SpaceFramework.Signal;
using UnityEngine;
namespace SpaceFramework.Mono
{
    public abstract class ASignalMono : MonoBehaviour, IConnectWithFramework, INeedInitialization<ASignalMono>
    {
        private void Awake()
        {
            Init();
        }
        SignalBehavior signalBehavior;
        public abstract IFrameworkBase GetFramework();
        public ASignalMono Init()
        {
            signalBehavior = new SignalBehavior().Init(GetFramework());
            InitAfterReady();
            return this;
        }
        /// <summary>
        /// 在环境初始化之后初始化
        /// 避免先初始化导致空引用
        /// </summary>
        protected virtual void InitAfterReady() { }
        public void SubscribeSignal<T>(string signalName, Action<T> action)
        {
            signalBehavior.SubscribeSignal(signalName, action);
        }
        public void UnsubscribeSignal(string signalName)
        { signalBehavior.UnsubscribeSignal(signalName); }
        public void SendSignal(string signalName, object singModel)
        { signalBehavior.SendSignal(signalName, singModel); }

    }
}
