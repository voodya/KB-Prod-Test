
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstaclesHolder", menuName = "Scriptable Objects/ObstaclesHolder")]
public class ObstaclesHolder : ScriptableObject
{
    [field: SerializeField] public List<Obstacle> _default;
    [field: SerializeField] public List<Obstacle> _eat;
    [field: SerializeField] public List<Obstacle> _kill;
}
