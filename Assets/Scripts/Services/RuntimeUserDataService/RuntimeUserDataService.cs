using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IRuntimeUserData
{
    IReactiveProperty<float> CurrentDistance { get; }
    IReactiveProperty<uint> CurrentEat { get; }
    IReactiveProperty<uint> CurrentHealth { get; }
    IReactiveProperty<int> PlayerPosition { get; }
    List<Vector2> RoadPositions { get; set; }
    List<RectTransform> RoadRects { get; set; }
    IReactiveProperty<bool[]> Grid { get; set; }
    float CurrentSpeed { get; set; }

    void SetStartData(uint distance, uint health, uint eat);
}


public class RuntimeUserDataService : IRuntimeUserData
{
    public IReactiveProperty<float> CurrentDistance { get; }
    public IReactiveProperty<uint> CurrentEat { get; }
    public IReactiveProperty<uint> CurrentHealth { get; }
    public IReactiveProperty<int> PlayerPosition { get; }
    public List<Vector2> RoadPositions { get; set; }
    public List<RectTransform> RoadRects { get; set; }
    public IReactiveProperty<bool[]> Grid { get; set; }
    public float CurrentSpeed { get; set; }

    public RuntimeUserDataService()
    {
        CurrentDistance = new ReactiveProperty<float>();
        CurrentEat = new ReactiveProperty<uint>();
        CurrentHealth = new ReactiveProperty<uint>();
        PlayerPosition = new ReactiveProperty<int>();
    }


    public void SetStartData(uint distance, uint health, uint eat)
    {
        PlayerPosition.Value = 1;
        CurrentDistance.Value = distance;
        CurrentHealth.Value = health;
        CurrentEat.Value = eat;
    }
}



