using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color dotColor;
    private Vector2Int gridPosition;
    private bool isConnected = false;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(Color color, Vector2Int position)
    {
        dotColor = color;
        gridPosition = position;
        spriteRenderer.color = color;
    }

    public Color GetDotColor()
    {
        return dotColor;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsConnected()
    {
        return isConnected;
    }

    public void SetConnected(bool connected)
    {
        isConnected = connected;
    }
}