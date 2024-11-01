using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    [SerializeField] private float cellSize = 1f;

    [Header("Prefabs")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject dotPrefab;

    [Header("Game Settings")]
    [SerializeField] private Color[] dotColors;
    [SerializeField] private LineManager lineManager;

    private Cell[,] grid;
    private Dictionary<Color, List<Dot>> colorToDots = new Dictionary<Color, List<Dot>>();
    private Camera mainCamera;
    private Dot currentDot;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (lineManager == null)
        {
            lineManager = GetComponent<LineManager>();
        }
    }

    private void Start()
    {
        GenerateGrid();
        PlaceTestDots();
    }

    private void GenerateGrid()
    {
        grid = new Cell[width, height];
        Transform gridHolder = new GameObject("Grid").transform;
        gridHolder.SetParent(transform);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity, gridHolder);
                cellObj.name = $"Cell_{x}_{y}";

                grid[x, y] = cellObj.GetComponent<Cell>();
            }
        }

        CenterGrid();
    }

    private void PlaceTestDots()
    {
        if (dotColors == null || dotColors.Length < 2)
        {
            Debug.LogError("Not enough colors defined!");
            return;
        }

        // Тестовые пары точек
        PlaceDotPair(new Vector2Int(0, 0), new Vector2Int(2, 2), dotColors[0]);
        PlaceDotPair(new Vector2Int(1, 1), new Vector2Int(3, 3), dotColors[1]);
    }

    private void PlaceDotPair(Vector2Int pos1, Vector2Int pos2, Color color)
    {
        Debug.Log($"Placing dot pair of color {color} at positions {pos1} and {pos2}");

        Dot dot1 = CreateDot(pos1, color);
        Dot dot2 = CreateDot(pos2, color);

        if (!colorToDots.ContainsKey(color))
        {
            colorToDots[color] = new List<Dot>();
        }

        colorToDots[color].Add(dot1);
        colorToDots[color].Add(dot2);
    }

    private Dot CreateDot(Vector2Int gridPos, Color color)
    {
        // Преобразуем позицию сетки в мировые координаты
        Vector3 worldPos = transform.position + new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, -0.1f);

        // Создаём точку как копию префаба
        GameObject dotObj = Instantiate(dotPrefab, worldPos, Quaternion.identity, transform);
        dotObj.name = $"Dot_{color}_{gridPos.x}_{gridPos.y}";

        Dot dot = dotObj.GetComponent<Dot>();
        dot.Initialize(color, gridPos);

        if (grid[gridPos.x, gridPos.y] != null)
        {
            grid[gridPos.x, gridPos.y].SetOccupied(true);
        }

        Debug.Log($"Created dot at {worldPos} with color {color}");
        return dot;
    }

    private void CenterGrid()
    {
        float offsetX = -(width * cellSize) / 2f + cellSize / 2f;
        float offsetY = -(height * cellSize) / 2f + cellSize / 2f;
        transform.position = new Vector3(offsetX, offsetY, 0);
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Dot hitDot = GetDotAtPosition(mousePosition);
            if (hitDot != null)
            {
                currentDot = hitDot;
                lineManager.StartLine(hitDot);
            }
        }
        else if (Input.GetMouseButton(0) && currentDot != null)
        {
            lineManager.UpdateLine(mousePosition);
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
}