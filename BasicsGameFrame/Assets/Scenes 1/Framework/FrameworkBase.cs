using System;
using SpaceFramework.Signal;
using TreeEditor;

namespace SpaceFramework.Base
{
    public abstract class AFrameworkBase<T> : IFrameworkBase where T : AFrameworkBase<T>, new()
    {
        public static IFrameworkBase FrameworkInterface => GetFrameworkInterface();
        public ISignalManager SignalManager => GetSignalManager();
        private static IFrameworkBase frameworkInstance;
        private static ISignalManager signalManager;
        protected static IFrameworkBase GetFrameworkInterface()
        {
            if (frameworkInstance == null)
                frameworkInstance = new T();
            return frameworkInstance;
        }
        protected static ISignalManager GetSignalManager()
        {
            if (signalManager == null)
                signalManager = new SignalManager().Init(GetFrameworkInterface());
            return signalManager;
        }
    }
    public interface IFrameworkBase
    {
        public static IFrameworkBase FrameworkInterface { get; }
        public ISignalManager SignalManager { get; }
    }
    public interface IConnectWithFramework
    {
        public IFrameworkBase GetFramework();
    }
    public interface INeedInitialization<T>
    {
        public T Init();
    }
    public interface IFrameworkComponent<T> : INeedInitialization<T>, IConnectWithFramework
    {
        public T Init(IFrameworkBase frameworkBase);
    }
    /// <summary>
    /// 框架组件的实例
    /// </summary>
    /// <typeparam name="T">T为对应的接口类</typeparam>
    public abstract class AFrameworkComponent<T> : IFrameworkComponent<T> where T : class
    {
        private IFrameworkBase frameworkBase;
        public IFrameworkBase GetFramework()
        {
            return frameworkBase;
        }
        public T Init(IFrameworkBase frameworkBase)
        {
            this.frameworkBase = frameworkBase;
            Init();
            return this as T;
        }
        public abstract T Init();
    }
}

