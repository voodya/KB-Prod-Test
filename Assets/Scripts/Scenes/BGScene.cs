using System.Collections.Generic;
using UnityEngine;

public class BGScene : ABaseScene
{
    [SerializeField] private List<RectTransform> _backLayer;
    [SerializeField] private List<RectTransform> _middleLayer;
    [SerializeField] private List<RectTransform> _frontLayer;

    public List<RectTransform> BackLayer => _backLayer;
    public List<RectTransform> MiddleLayer => _middleLayer;
    public List<RectTransform> FrontLayer => _frontLayer;
}
