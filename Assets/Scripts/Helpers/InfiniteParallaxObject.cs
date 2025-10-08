using UnityEngine;

public class InfiniteParallaxObject : MonoBehaviour
{
    [Tooltip("The speed at which this object moves. Use different values for each layer.")]
    [SerializeField] private float scrollSpeed = 50f;

    private RectTransform rectTransform;
    private float imageWidth;

    void Start()
    {
        // Получаем компонент RectTransform этого UI-объекта
        rectTransform = GetComponent<RectTransform>();

        // Вычисляем ширину нашего изображения.
        // Это важно для определения момента, когда нужно "телепортироваться".
        imageWidth = rectTransform.rect.width;
    }

    void Update()
    {
        // Двигаем объект влево с заданной скоростью.
        // Используем Vector2.left как направление (-1, 0).
        rectTransform.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        // Проверяем, ушел ли объект полностью за левый край экрана.
        // Мы проверяем, если правый край объекта (позиция + ширина) находится левее нуля.
        // Для надежности берем позицию центра и добавляем половину ширины.
        if (rectTransform.anchoredPosition.x < -imageWidth)
        {
            // Если да, то перемещаем его вправо на расстояние,
            // равное ширине ДВУХ изображений (своего и дубликата).
            Vector2 newPos = rectTransform.anchoredPosition;
            newPos.x += 2 * imageWidth;
            rectTransform.anchoredPosition = newPos;
        }
    }
}
