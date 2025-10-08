using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    public RectTransform Rect => _rect;
    public Image View => _image;
}


