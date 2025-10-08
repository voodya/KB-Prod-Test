using UnityEngine;

public class CharacterViewScene : ABaseScene
{
    [SerializeField] private RectTransform _characterRect;

    public RectTransform CharacterRect => _characterRect;
}
