using UnityEngine;

[CreateAssetMenu(menuName = "SO/Configs/Obstacle", fileName = "Obstacle info")]
public class ObstacleConfig : ScriptableObject
{
    public EObstacleType Type;
    public uint Points;
    public Sprite Sprite;
}

public enum EObstacleType
{
    DefaultObstacle,
    FatalObstacle,
    Eat,
    None
}
