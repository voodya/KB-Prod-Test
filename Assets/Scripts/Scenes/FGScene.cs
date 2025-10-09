using System.Collections.Generic;
using UnityEngine;

public class FGScene : ABaseScene
{
    [SerializeField] private List<RectTransform> _frontLayer;
    public List<RectTransform> ExtraFrontLayer => _frontLayer;
}
