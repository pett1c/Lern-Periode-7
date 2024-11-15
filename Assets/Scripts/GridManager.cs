using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private LineManager lineManager;

    [Header("Game Settings")]
    [SerializeField]
    private Color[] dotColors = new Color[]
    {
    new Color(1f, 0f, 0f), // Красный
    new Color(0f, 0f, 1f), // Синий
    new Color(0f, 1f, 0f), // Зеленый
    new Color(1f, 1f, 0f), // Желтый
    new Color(1f, 0f, 1f), // Пурпурный
    };

    private Cell[,] grid;
    private Dictionary<Color, List<Dot>> colorToDots = new Dictionary<Color, List<Dot>>();
    private Camera mainCamera;
    private Dot currentDot;
    public int Width => width;
    public int Height => height;

    private void Start()
    {
        GenerateGrid();
        PlaceDotsForTesting(); // Временный метод для тестирования
    }
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleInput();
    }

    public bool AreAllDotsConnected()
    {
        return FindObjectsOfType<Dot>().All(dot => dot.IsConnected());
    }

    public bool IsGridFull()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!grid[x, y].IsOccupied())
                    return false;
            }
        }
        return true;
    }

    public Dot GetDotAtGridPosition(Vector2Int gridPos)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(GridToWorldPosition(gridPos));
        foreach (Collider2D collider in colliders)
        {
            Dot dot = collider.GetComponent<Dot>();
            if (dot != null)
                return dot;
        }
        return null;
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, 0) + transform.position;
    }

    public Vector2Int WorldToGridPosition(Vector2 worldPos)
    {
        Vector2 localPos = worldPos - (Vector2)transform.position;
        return new Vector2Int(
            Mathf.RoundToInt(localPos.x / cellSize),
            Mathf.RoundToInt(localPos.y / cellSize)
        );
    }

    private void HandleInput()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPosition = WorldToGridPosition(mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Dot hitDot = GetDotAtPosition(mousePosition);
            if (hitDot != null)
            {
                currentDot = hitDot;
                lineManager.StartLine(hitDot, gridPosition);
            }
        }
        else if (Input.GetMouseButton(0) && currentDot != null)
        {
            lineManager.UpdateLine(gridPosition);
        }
        else if (Input.GetMouseButtonUp(0) && currentDot != null)
        {
            Dot endDot = GetDotAtPosition(mousePosition);
            lineManager.EndLine(endDot);
            currentDot = null;
        }
    }

    private Dot GetDotAtPosition(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(position);
        foreach (Collider2D collider in colliders)
        {
            Dot dot = collider.GetComponent<Dot>();
            if (dot != null)
                return dot;
        }
        return null;
    }
    private void GenerateGrid()
    {
        grid = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject cellObject = Instantiate(cellPrefab, position, Quaternion.identity);
                cellObject.transform.parent = transform;
                cellObject.name = $"Cell ({x}, {y})";

                grid[x, y] = cellObject.GetComponent<Cell>();
            }
        }

        CenterGrid();
    }

    private void PlaceDotsForTesting()
    {
        // Красная пара
        PlaceDotPair(new Vector2Int(0, 4), new Vector2Int(4, 4), dotColors[0]);

        // Синяя пара
        PlaceDotPair(new Vector2Int(0, 3), new Vector2Int(4, 3), dotColors[1]);

        // Желтая пара
        PlaceDotPair(new Vector2Int(0, 2), new Vector2Int(3, 2), dotColors[2]);

        // Зеленая пара
        PlaceDotPair(new Vector2Int(0, 0), new Vector2Int(4, 0), dotColors[3]);

        // Фиолетовая пара
        PlaceDotPair(new Vector2Int(4, 2), new Vector2Int(0, 1), dotColors[4]);
    }

    private void PlaceDotPair(Vector2Int pos1, Vector2Int pos2, Color color)
    {
        // Создаем первую точку
        Dot dot1 = CreateDot(pos1, color);
        // Создаем вторую точку
        Dot dot2 = CreateDot(pos2, color);

        // Добавляем точки в словарь
        if (!colorToDots.ContainsKey(color))
        {
            colorToDots[color] = new List<Dot>();
        }
        colorToDots[color].Add(dot1);
        colorToDots[color].Add(dot2);
    }

    private Dot CreateDot(Vector2Int gridPos, Color color)
    {
        Vector3 worldPos = new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, -0.1f) + transform.position;
        GameObject dotObject = Instantiate(dotPrefab, worldPos, Quaternion.identity, transform);
        Dot dot = dotObject.GetComponent<Dot>();
        dot.Initialize(color, gridPos);
        grid[gridPos.x, gridPos.y].SetOccupied(true);
        return dot;
    }

    private void CenterGrid()
    {
        float offsetX = -(width * cellSize) / 2f + cellSize / 2f;
        float offsetY = -(height * cellSize) / 2f + cellSize / 2f;
        transform.position = new Vector3(offsetX, offsetY, 0);
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return grid[x, y];
        return null;
    }
}