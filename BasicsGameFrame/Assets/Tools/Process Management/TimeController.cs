using System.Collections.Generic;
using StateNode;
using Tool;
using UnityEngine;
#region ��Ϸ��������¼�
/// <summary>
/// ��ͣ��Ϸʱ��(ֹͣ����)
/// </summary>
public struct Pause : EventModel { }
/// <summary>
/// ������Ϸʱ��
/// </summary>
public struct Continue : EventModel { }
/// <summary>
/// ֹͣ���н���(ȫ����ͣ)
/// (��Ϊ��ʱ����Ϸʱ����ͣ�˵��ǿ��Խ����жϼ��һ��ģ�������ʱ����Ϸ������ͣ�ģ�)
/// </summary> 
public struct Stop : EventModel { }
#endregion
public class TimeController : Singleton<TimeController>
{
    #region ��������
    /// <summary>
    /// ��Ϸ����ʵ��ȥ��ʱ��(ȥ����ͣ)
    /// </summary>
    public float Time => time;
    public int GameRounds => gameRounds;
    #endregion
    [SerializeField] bool pause = false;
    [SerializeField] bool stop = false;
    private float time = 0;
    private int gameRounds = 0;

    #region ��Ҫ���¼��嵥
    Dictionary<int, StateNote> StateNodeDic = new Dictionary<int, StateNote>();
    public StateNote tempNode;//���ڵ�״̬
    #endregion
    #region ��ʼ����ˢ��
    private void Start()
    {
        time = 0;
        pause = false;
        List<StateNote> stateNotes = new List<StateNote>()
        {
            new StartRound().SetID(0).NextID(1),
            new StayRound().SetID(1).NextID(2),
            new ExitRound().SetID(2).NextID(3),
            new NextRound().SetID(3).NextID(0),
        };
        foreach (var note in stateNotes)
        {
            StateNodeDic.Add(note.ID, note);
        }
        tempNode = StateNodeDic[0];//���ó�ʼ
        ZEventSystem.Instance.SubscribeEvent<Pause>((v) =>
        {
            pause = true;
        });
        ZEventSystem.Instance.SubscribeEvent<Continue>((v) =>
        {
            pause = false;
            stop = false;
        });
        ZEventSystem.Instance.SubscribeEvent<Stop>((V) =>
        {
            pause = true;
            stop = true;
        });
        ZEventSystem.Instance.SubscribeEvent<NextRoundEvent>((v) => { AddRound(); });
    }
    private void Update()
    {
        if (!stop)
        {
            if (!pause)
            {
                time += UnityEngine.Time.deltaTime;
                tempNode?.Update();
            }
            tempNode?.UpdateBeyondTime();
        }
    }
    #endregion
    #region ���к���
    public bool Next()
    {
        if (tempNode.CouldNext)
        {
            tempNode.IExit();
            tempNode = StateNodeDic[tempNode.NextState()];
            tempNode.Enter();
            return true;
        }
        return false;
    }
    public bool ISPause() { return pause; }
    public bool ISStop() { return stop; }
    /// <summary>
    /// �غϼ�һ���������ڵĻغ���
    /// </summary>
    /// <returns>����</returns>
    public int AddRound()
    {
        gameRounds++;
        return gameRounds;
    }
    #endregion
}