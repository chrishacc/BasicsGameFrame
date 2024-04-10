using Tool;
namespace StateNode
{
    #region ����
    public class StateNote
    {
        public bool CouldNext = false;
        private int NextStateID;
        public int ID;
        public StateNote NextID(int id) { NextStateID = id; return this; }
        public StateNote SetID(int id) { ID = id; return this; }
        public int NextState() { return NextStateID; }
        /// <summary>
        /// ֻ����Ϸʱ��������ʱ�����
        /// </summary>
        public virtual void Update()
        { }
        /// <summary>
        /// ��ʹ��Ϸʱ����ͣҲ�����
        /// </summary>
        public virtual void UpdateBeyondTime()
        { }
        public virtual void Enter() { }
        public void IExit() { CouldNext = false; Exit(); }//ȷ��ÿ���뿪��״̬����
        protected virtual void Exit() { }
    }
    #endregion
    #region �����µ�һ��
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
    #region  �µ�һ��
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
    #region ������һ��
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
    #region ������һ��ѭ��
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
