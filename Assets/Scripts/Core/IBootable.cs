using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBootable 
{
    int Priority { get; }
    UniTask Boot();
}
