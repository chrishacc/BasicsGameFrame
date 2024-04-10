using System.Collections.Generic;
using StateNode;
using Tool;
using UnityEngine;
#region 游戏进程相关事件
/// <summary>
/// 暂停游戏时间(停止生产)
/// </summary>
public struct Pause : EventModel { }
/// <summary>
/// 继续游戏时间
/// </summary>
public struct Continue : EventModel { }
/// <summary>
/// 停止所有进程(全部暂停)
/// (因为有时候游戏时间暂停了但是可以进行判断检测一类的（如结算的时候游戏是算暂停的）)
/// </summary> 
public struct Stop : EventModel { }
#endregion
public class TimeController : Singleton<TimeController>
{
    #region 公有数据
    /// <summary>
    /// 游戏内真实过去的时间(去除暂停)
    /// </summary>
    public float Time => time;
    public int GameRounds => gameRounds;
    #endregion
    [SerializeField] bool pause = false;
    [SerializeField] bool stop = false;
    private float time = 0;
    private int gameRounds = 0;

    #region 主要的事件清单
    Dictionary<int, StateNote> StateNodeDic = new Dictionary<int, StateNote>();
    public StateNote tempNode;//现在的状态
    #endregion
    #region 初始化和刷新
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
        tempNode = StateNodeDic[0];//设置初始
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
    #region 公有函数
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
    /// 回合加一并返回现在的回合数
    /// </summary>
    /// <returns>返回</returns>
    public int AddRound()
    {
        gameRounds++;
        return gameRounds;
    }
    #endregion
}