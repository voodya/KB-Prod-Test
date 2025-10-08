using UnityEngine;

public class InfiniteParallaxObject : MonoBehaviour
{
    [Tooltip("The speed at which this object moves. Use different values for each layer.")]
    [SerializeField] private float scrollSpeed = 50f;

    private RectTransform rectTransform;
    private float imageWidth;

    void Start()
    {
        // �������� ��������� RectTransform ����� UI-�������
        rectTransform = GetComponent<RectTransform>();

        // ��������� ������ ������ �����������.
        // ��� ����� ��� ����������� �������, ����� ����� "�����������������".
        imageWidth = rectTransform.rect.width;
    }

    void Update()
    {
        // ������� ������ ����� � �������� ���������.
        // ���������� Vector2.left ��� ����������� (-1, 0).
        rectTransform.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        // ���������, ���� �� ������ ��������� �� ����� ���� ������.
        // �� ���������, ���� ������ ���� ������� (������� + ������) ��������� ����� ����.
        // ��� ���������� ����� ������� ������ � ��������� �������� ������.
        if (rectTransform.anchoredPosition.x < -imageWidth)
        {
            // ���� ��, �� ���������� ��� ������ �� ����������,
            // ������ ������ ���� ����������� (������ � ���������).
            Vector2 newPos = rectTransform.anchoredPosition;
            newPos.x += 2 * imageWidth;
            rectTransform.anchoredPosition = newPos;
        }
    }
}
