using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Dot : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2Int gridPosition;
    private bool isConnected;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Color color, Vector2Int position)
    {
        gridPosition = position;

        // �����: ���������� ��� �� ��������, ��� � � �������
        if (spriteRenderer.material != null)
        {
            // ������ ����� ���������, ����� �� ������ ������������
            spriteRenderer.material = new Material(spriteRenderer.material);
        }

        spriteRenderer.color = color;
        name = $"Dot_{color.ToString()}_{position.x}_{position.y}";

        // ��������, ��� ����� ��������� ����� �����
        spriteRenderer.sortingOrder = 2;
    }

    public Color GetColor() => spriteRenderer.color;
    public Vector2Int GetPosition() => gridPosition;
    public bool IsConnected() => isConnected;
    public void SetConnected(bool value) => isConnected = value;
}