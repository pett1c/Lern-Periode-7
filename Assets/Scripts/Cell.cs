using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isOccupied = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsOccupied()
    {
        return isOccupied;
    }

    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
    }
}