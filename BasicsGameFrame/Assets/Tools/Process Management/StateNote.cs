using Tool;
namespace StateNode
{
    #region 基类
    public class StateNote
    {
        public bool CouldNext = false;
        private int NextStateID;
        public int ID;
        public StateNote NextID(int id) { NextStateID = id; return this; }
        public StateNote SetID(int id) { ID = id; return this; }
        public int NextState() { return NextStateID; }
        /// <summary>
        /// 只有游戏时间在运行时会调用
        /// </summary>
        public virtual void Update()
        { }
        /// <summary>
        /// 即使游戏时间暂停也会调用
        /// </summary>
        public virtual void UpdateBeyondTime()
        { }
        public virtual void Enter() { }
        public void IExit() { CouldNext = false; Exit(); }//确保每次离开把状态重置
        protected virtual void Exit() { }
    }
    #endregion
    #region 进入新的一天
    public struct StartRoundEvent : EventModel { }
    public class StartRound : StateNote
    {
        public override void Enter()
        {
            base.Enter();
            ZEventSystem.Instance.SendEvent<StartRoundEvent>();
            CouldNext = true;
        }
    }
    #endregion
    #region  新的一天
    public struct StayRoundEvent : EventModel { }
    public class StayRound : StateNote
    {
        float enterTime;
        float StayTime = 3f;
        public override void Enter()
        {
            base.Enter();
            ZEventSystem.Instance.SendEvent<Continue>();
            ZEventSystem.Instance.SendEvent<StayRoundEvent>();
            enterTime = TimeController.Instance.Time;
        }
        public override void Update()
        {
            if (!CouldNext && TimeController.Instance.Time - enterTime > StayTime)
            {
                CouldNext = true;
            }
        }
        protected override void Exit()
        {
            base.Exit();
            ZEventSystem.Instance.SendEvent<Pause>();
        }
    }
    #endregion
    #region 结束这一天
    public struct ExitRoundEvent : EventModel { }
    public class ExitRound : StateNote
    {
        float enterTime;
        float StayTime = 30f;
        public override void Enter()
        {
            base.Enter();
            ZEventSystem.Instance.SendEvent<StayRoundEvent>();
            enterTime = TimeController.Instance.Time;
        }
    }
    #endregion
    #region 进入下一个循环
    public struct NextRoundEvent : EventModel { }
    public class NextRound : StateNote
    {
        public override void Enter()
        {
            ZEventSystem.Instance.SendEvent<NextRoundEvent>();
        }
    }
    #endregion
}
