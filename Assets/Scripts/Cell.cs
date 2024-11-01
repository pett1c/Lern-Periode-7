using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isOccupied;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white; // ��� ����� ������ ���� ��� �����
    }

    public bool IsOccupied() => isOccupied;
    public void SetOccupied(bool occupied) => isOccupied = occupied;
}