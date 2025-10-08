using UnityEngine;

[CreateAssetMenu(menuName = "SO/Configs/GameConfig", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    public uint StartLivesCount;
    public uint ObjectSpawnRate;
    public uint StartRowsCount;
    public uint MaxRowsCount;
    public uint AddRowsRate;
    public float StratSpeed;
    public float CoeffSpeed;
    public uint SpeedChangeRate;
    public float MaxSpeed => StratSpeed * 3f;
}
