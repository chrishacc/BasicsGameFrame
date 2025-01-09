using SpaceFramework.Base;
using SpaceFramework.Mono;
using UnityEngine;

class SignalTest : ASignalMono
{
    protected override void InitAfterReady()
    {
        base.InitAfterReady();
        SubscribeSignal<float>("Test", (a) => { Debug.Log("Number:" + a.ToString()); });
    }
    private void Update()
    {
        SendSignal("Test", Time.time);
    }
    public override IFrameworkBase GetFramework()
    {
        return GameFrameWork.FrameworkInterface;
    }
}