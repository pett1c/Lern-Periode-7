using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class HintPathData
{
    public Color color;
    public List<Vector2Int> path = new List<Vector2Int>();
}

public class GameUIManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LineManager lineManager;
    [SerializeField] private MoveCounter moveCounter;

    [Header("UI Elements")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button hintButton;
    [SerializeField] private Button editHintButton;

    [Header("Hint Settings")]
    [SerializeField] private LineRenderer hintLineRenderer;
    [SerializeField] private float hintDuration = 2f;
    [SerializeField] private List<HintPathData> savedHintPaths = new List<HintPathData>();

    private bool isHintActive = false;
    private float hintTimer = 0f;
    private bool isEditingHint = false;
    private List<Vector2Int> currentEditingPath = new List<Vector2Int>();
    private Color currentEditingColor;
    private Dot startDot;
    private Vector2Int lastPosition;

    private void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);

        if (hintButton != null)
            hintButton.onClick.AddListener(ShowNextHint);

        if (editHintButton != null)
            editHintButton.onClick.AddListener(ToggleHintEditor);

        if (hintLineRenderer != null)
        {
            hintLineRenderer.gameObject.SetActive(false);
        }

        LoadHints(); // Загружаем сохранённые подсказки при старте
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock) && editHintButton != null)
        {
            editHintButton.gameObject.SetActive(!editHintButton.gameObject.activeSelf);
        }

        if (isHintActive && !isEditingHint)
        {
            hintTimer -= Time.deltaTime;
            if (hintTimer <= 0)
            {
                HideHint();
            }
        }

        if (isEditingHint)
        {
            HandleHintEditing();
        }
    }

    private void HandleHintEditing()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPosition = gridManager.WorldToGridPosition(mousePosition);

        // Начало редактирования
        if (Input.GetMouseButtonDown(0))
        {
            Dot clickedDot = gridManager.GetDotAtPosition(mousePosition);
            if (clickedDot != null && !clickedDot.IsConnected())
            {
                startDot = clickedDot;
                currentEditingColor = clickedDot.GetDotColor();
                currentEditingPath.Clear();
                currentEditingPath.Add(clickedDot.GetGridPosition());
                lastPosition = clickedDot.GetGridPosition();

                // Настраиваем и показываем LineRenderer
                hintLineRenderer.gameObject.SetActive(true);
                hintLineRenderer.startColor = currentEditingColor;
                hintLineRenderer.endColor = currentEditingColor;
                UpdateHintLine(currentEditingPath);
            }
        }
        // Продолжение рисования
        else if (Input.GetMouseButton(0) && startDot != null)
        {
            if (IsValidMove(lastPosition, gridPosition))
            {
                if (!currentEditingPath.Contains(gridPosition))
                {
                    // Проверяем, не заканчивается ли путь на точке того же цвета
                    Dot endDot = gridManager.GetDotAtGridPosition(gridPosition);
                    if (endDot != null)
                    {
                        if (endDot.GetDotColor() == currentEditingColor && endDot != startDot)
                        {
                            currentEditingPath.Add(gridPosition);
                            UpdateHintLine(currentEditingPath);
                            SaveCurrentPath();
                            isEditingHint = false;
                            return;
                        }
                        else if (endDot != startDot)
                        {
                            return;
                        }
                    }

                    currentEditingPath.Add(gridPosition);
                    lastPosition = gridPosition;
                    UpdateHintLine(currentEditingPath);
                }
            }
        }
        // Отмена последнего действия
        if (Input.GetMouseButtonDown(1) && currentEditingPath.Count > 1)
        {
            currentEditingPath.RemoveAt(currentEditingPath.Count - 1);
            lastPosition = currentEditingPath[currentEditingPath.Count - 1];
            UpdateHintLine(currentEditingPath);
        }
    }

    private void SaveCurrentPath()
    {
        // Проверяем, существует ли уже путь для этого цвета
        HintPathData existingPath = savedHintPaths.FirstOrDefault(p => p.color == currentEditingColor);

        if (existingPath != null)
        {
            // Обновляем существующий путь
            existingPath.path = new List<Vector2Int>(currentEditingPath);
        }
        else
        {
            // Создаем новый путь
            HintPathData newPath = new HintPathData
            {
                color = currentEditingColor,
                path = new List<Vector2Int>(currentEditingPath)
            };
            savedHintPaths.Add(newPath);
        }

        // Сохраняем подсказки в PlayerPrefs
        SaveHints();
    }

    private bool IsValidMove(Vector2Int from, Vector2Int to)
    {
        Vector2Int delta = to - from;
        return (Mathf.Abs(delta.x) + Mathf.Abs(delta.y) == 1) &&
               to.x >= 0 && to.x < gridManager.Width &&
               to.y >= 0 && to.y < gridManager.Height;
    }

    private void UpdateHintLine(List<Vector2Int> path)
    {
        hintLineRenderer.positionCount = path.Count;
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 worldPos = gridManager.GridToWorldPosition(path[i]);
            hintLineRenderer.SetPosition(i, new Vector3(worldPos.x, worldPos.y, -0.3f));
        }
    }

    public void ToggleHintEditor()
    {
        isEditingHint = !isEditingHint;
        if (isEditingHint)
        {
            currentEditingPath.Clear();
            hintLineRenderer.gameObject.SetActive(true);
        }
        else
        {
            hintLineRenderer.gameObject.SetActive(false);
        }
    }

    private int currentHintIndex = -1;

    public void ShowNextHint()
    {
        if (isEditingHint || savedHintPaths.Count == 0)
            return;

        currentHintIndex = (currentHintIndex + 1) % savedHintPaths.Count;
        var hintData = savedHintPaths[currentHintIndex];

        hintLineRenderer.gameObject.SetActive(true);
        hintLineRenderer.startColor = hintData.color;
        hintLineRenderer.endColor = hintData.color;
        UpdateHintLine(hintData.path);

        isHintActive = true;
        hintTimer = hintDuration;
    }

    public void RestartLevel()
    {
        var lines = FindObjectsOfType<Line>();
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }

        var dots = FindObjectsOfType<Dot>();
        foreach (var dot in dots)
        {
            dot.SetConnected(false);
        }

        for (int x = 0; x < gridManager.Width; x++)
        {
            for (int y = 0; y < gridManager.Height; y++)
            {
                var cell = gridManager.GetCell(x, y);
                if (cell != null)
                {
                    var dotAtPosition = gridManager.GetDotAtGridPosition(new Vector2Int(x, y));
                    cell.SetOccupied(dotAtPosition != null);
                }
            }
        }

        var field = typeof(LineManager).GetField("moveCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            field.SetValue(lineManager, 0);

        moveCounter?.UpdateMoveDisplay(0);
        HideHint();
        currentHintIndex = -1;  // Сбрасываем индекс подсказки
    }

    private void HideHint()
    {
        if (!isEditingHint)
        {
            isHintActive = false;
            hintLineRenderer.gameObject.SetActive(false);
        }
    }

    // Опционально: метод для сохранения подсказок в PlayerPrefs
    private void SaveHints()
    {
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        int pathCount = savedHintPaths.Count;
        PlayerPrefs.SetInt($"{levelName}_HintCount", pathCount);

        for (int i = 0; i < pathCount; i++)
        {
            var hintData = savedHintPaths[i];
            string colorKey = $"{levelName}_Hint_{i}_Color";
            string pathKey = $"{levelName}_Hint_{i}_Path";

            // Сохраняем цвет
            PlayerPrefs.SetFloat($"{colorKey}_R", hintData.color.r);
            PlayerPrefs.SetFloat($"{colorKey}_G", hintData.color.g);
            PlayerPrefs.SetFloat($"{colorKey}_B", hintData.color.b);

            // Сохраняем путь
            string pathData = string.Join("|", hintData.path.Select(p => $"{p.x},{p.y}"));
            PlayerPrefs.SetString(pathKey, pathData);
        }

        PlayerPrefs.Save();
    }

    // Опционально: метод для загрузки подсказок из PlayerPrefs
    private void LoadHints()
    {
        string levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        int pathCount = PlayerPrefs.GetInt($"{levelName}_HintCount", 0);
        savedHintPaths.Clear();

        for (int i = 0; i < pathCount; i++)
        {
            string colorKey = $"{levelName}_Hint_{i}_Color";
            string pathKey = $"{levelName}_Hint_{i}_Path";

            // Загружаем цвет
            Color color = new Color(
                PlayerPrefs.GetFloat($"{colorKey}_R"),
                PlayerPrefs.GetFloat($"{colorKey}_G"),
                PlayerPrefs.GetFloat($"{colorKey}_B")
            );

            // Загружаем путь
            string pathData = PlayerPrefs.GetString(pathKey);
            List<Vector2Int> path = pathData.Split('|')
                .Select(p => {
                    string[] coords = p.Split(',');
                    return new Vector2Int(int.Parse(coords[0]), int.Parse(coords[1]));
                })
                .ToList();

            savedHintPaths.Add(new HintPathData { color = color, path = path });
        }
    }
}