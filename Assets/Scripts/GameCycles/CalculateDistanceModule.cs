using UnityEngine;
/// <summary>
/// Just update current distance
/// </summary>
public class CalculateDistanceModule : ABaseModule
{
    public CalculateDistanceModule(IRuntimeUserData runtimeUserData) : base(runtimeUserData)
    {
    }

    public override void OnUpdate()
    {
        _runtimeData.CurrentDistance.Value += _runtimeData.CurrentSpeed * Time.deltaTime;
    }
}
