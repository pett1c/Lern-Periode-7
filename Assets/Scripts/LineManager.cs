using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GridManager gridManager;
    private Line currentLine;
    private Dot activeDot;
    private Vector2Int lastGridPosition;
    private List<Vector2Int> linePath = new List<Vector2Int>();
    private Dictionary<Color, Line> colorToLine = new Dictionary<Color, Line>();
    private Dictionary<Color, (Dot, Dot)> connectedDots = new Dictionary<Color, (Dot, Dot)>();
    private Dictionary<Color, List<Vector2Int>> colorToPaths = new Dictionary<Color, List<Vector2Int>>();
    public event System.Action OnLineCompleted;

    private void Start()
    {
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();
    }

    public void StartLine(Dot dot, Vector2Int gridPosition)
    {
        // Проверяем, является ли точка уже соединенной
        if (dot.IsConnected())
        {
            RemoveLine(dot.GetDotColor());
            return;
        }

        activeDot = dot;
        lastGridPosition = gridPosition;
        linePath.Clear();
        linePath.Add(gridPosition);

        // Если уже существует линия этого цвета, удаляем её
        if (colorToLine.ContainsKey(dot.GetDotColor()))
        {
            RemoveLine(dot.GetDotColor());
        }

        // Создаем новую линию
        GameObject lineObj = Instantiate(linePrefab, transform);
        currentLine = lineObj.GetComponent<Line>();
        currentLine.SetColor(dot.GetDotColor());
        currentLine.AddPoint(gridManager.GridToWorldPosition(gridPosition));
        colorToLine[dot.GetDotColor()] = currentLine;
    }

    private void RemoveLine(Color color)
    {
        if (colorToLine.ContainsKey(color))
        {
            // Удаляем объект линии
            Destroy(colorToLine[color].gameObject);
            colorToLine.Remove(color);

            // Освобождаем клетки на пути линии
            if (connectedDots.ContainsKey(color))
            {
                var (dot1, dot2) = connectedDots[color];
                dot1.SetConnected(false);
                dot2.SetConnected(false);
                connectedDots.Remove(color);
            }

            // Освобождаем все клетки на сохраненном пути
            if (colorToPaths.ContainsKey(color))
            {
                foreach (var pos in colorToPaths[color])
                {
                    var cell = gridManager.GetCell(pos.x, pos.y);
                    if (cell != null)
                    {
                        // Проверяем, нет ли на клетке точки или другой линии
                        var dotAtCell = gridManager.GetDotAtGridPosition(pos);
                        if (dotAtCell == null)
                        {
                            // Проверяем, не проходит ли через эту клетку другая линия
                            bool isUsedByOtherLine = false;
                            foreach (var path in colorToPaths.Where(kvp => kvp.Key != color))
                            {
                                if (path.Value.Contains(pos))
                                {
                                    isUsedByOtherLine = true;
                                    break;
                                }
                            }
                            if (!isUsedByOtherLine)
                            {
                                cell.SetOccupied(false);
                            }
                        }
                    }
                }
                colorToPaths.Remove(color);
            }
        }
    }

    public void UpdateLine(Vector2Int newGridPosition)
    {
        if (currentLine == null || activeDot == null) return;

        // Проверка границ поля
        if (newGridPosition.x < 0 || newGridPosition.x >= gridManager.Width ||
            newGridPosition.y < 0 || newGridPosition.y >= gridManager.Height)
            return;

        Vector2Int delta = newGridPosition - lastGridPosition;
        if (Mathf.Abs(delta.x) + Mathf.Abs(delta.y) != 1) return;

        // Проверка на движение назад
        if (linePath.Count > 1 && newGridPosition == linePath[linePath.Count - 2])
        {
            linePath.RemoveAt(linePath.Count - 1);
            lastGridPosition = newGridPosition;
            currentLine.RemoveLastPoint();
            return;
        }

        // Проверка на начальную точку
        if (newGridPosition == activeDot.GetGridPosition())
            return;

        // Проверка на пересечение с текущей линией
        if (linePath.Contains(newGridPosition))
            return;

        // Проверка на столкновение с точкой и занятость клетки
        Dot dotAtPosition = gridManager.GetDotAtGridPosition(newGridPosition);
        Cell cell = gridManager.GetCell(newGridPosition.x, newGridPosition.y);

        if (cell != null && cell.IsOccupied())
        {
            // Проверяем, не точка ли это такого же цвета
            if (dotAtPosition == null || dotAtPosition.GetDotColor() != activeDot.GetDotColor())
                return;
        }

        if (dotAtPosition != null)
        {
            if (dotAtPosition.GetDotColor() != activeDot.GetDotColor())
                return; // Блокируем движение если точка другого цвета

            if (dotAtPosition.GetDotColor() == activeDot.GetDotColor() && dotAtPosition != activeDot)
            {
                currentLine.AddPoint(gridManager.GridToWorldPosition(newGridPosition));
                linePath.Add(newGridPosition);
                EndLine(dotAtPosition);
                return;
            }
        }

        currentLine.AddPoint(gridManager.GridToWorldPosition(newGridPosition));
        linePath.Add(newGridPosition);
        lastGridPosition = newGridPosition;
    }
    public void EndLine(Dot endDot)
    {
        if (currentLine != null && activeDot != null)
        {
            if (endDot != null && endDot.GetDotColor() == activeDot.GetDotColor() && endDot != activeDot)
            {
                activeDot.SetConnected(true);
                endDot.SetConnected(true);

                // Сохраняем пару соединенных точек
                connectedDots[activeDot.GetDotColor()] = (activeDot, endDot);

                // Сохраняем путь линии
                colorToPaths[activeDot.GetDotColor()] = new List<Vector2Int>(linePath);

                // Отмечаем все клетки по пути как занятые
                foreach (var pos in linePath)
                {
                    gridManager.GetCell(pos.x, pos.y).SetOccupied(true);
                }
                OnLineCompleted?.Invoke();
            }
            else
            {
                Destroy(currentLine.gameObject);
                colorToLine.Remove(activeDot.GetDotColor());
            }
        }

        currentLine = null;
        activeDot = null;
        linePath.Clear();
    }
}