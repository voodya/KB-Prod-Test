using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _imageRect;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private EObstacleType _obstacleType;
    private ObstacleConfig _cachedConfig;
    public RectTransform Rect => _rect;
    public Image View => _image;
    public EObstacleType ObstacleType => _cachedConfig.Type;
    public uint Points => _cachedConfig.Points;

    public void Configure(ObstacleConfig config, CustomBoxCollider2DRect customSize)
    {
        _obstacleType = config.Type;
        _image.sprite = config.Sprite;
        _cachedConfig = config;
        _boxCollider.size = customSize.Size;
        _boxCollider.offset = customSize.Offset;
        _imageRect.sizeDelta = customSize.Size;
        
        
    }
}

public struct CustomBoxCollider2DRect
{
    public Vector2 Size;
    public Vector2 Offset;
}








