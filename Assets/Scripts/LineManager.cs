using UnityEngine;
using System.Collections.Generic;

public class LineManager : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    private Dictionary<Color, Line> activeLines = new Dictionary<Color, Line>();
    private Line currentLine;
    private Dot startDot;

    public void StartLine(Dot dot)
    {
        if (dot == null) return;

        Color dotColor = dot.GetColor();
        startDot = dot;

        // Удаляем существующую линию того же цвета
        if (activeLines.ContainsKey(dotColor))
        {
            Destroy(activeLines[dotColor].gameObject);
            activeLines.Remove(dotColor);
        }

        // Создаём новую линию
        GameObject lineObj = Instantiate(linePrefab, transform);
        currentLine = lineObj.GetComponent<Line>();
        currentLine.SetColor(dotColor);
        currentLine.AddPoint(dot.transform.position);

        activeLines[dotColor] = currentLine;
    }

    public void UpdateLine(Vector2 position)
    {
        if (currentLine != null)
        {
            currentLine.UpdateLastPoint(position);
        }
    }

    public void EndLine(Dot endDot)
    {
        if (currentLine != null && startDot != null)
        {
            if (endDot != null && endDot.GetColor() == startDot.GetColor() && endDot != startDot)
            {
                // Успешное соединение
                currentLine.AddPoint(endDot.transform.position);
                startDot.SetConnected(true);
                endDot.SetConnected(true);
            }
            else
            {
                // Неуспешное соединение
                Color lineColor = currentLine.GetColor();
                Destroy(currentLine.gameObject);
                if (activeLines.ContainsKey(lineColor))
                {
                    activeLines.Remove(lineColor);
                }
            }
        }

        currentLine = null;
        startDot = null;
    }
}