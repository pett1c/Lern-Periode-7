using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Line : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector2> points = new List<Vector2>();
    private Color lineColor;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingOrder = 1; // Линии под точками
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 0;
    }

    public void SetColor(Color color)
    {
        lineColor = color;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        // Создаём копию материала для каждой линии
        if (lineRenderer.material != null)
        {
            lineRenderer.material = new Material(lineRenderer.material);
        }
    }

    public void AddPoint(Vector2 point)
    {
        points.Add(point);
        UpdateLine();
    }

    public void UpdateLastPoint(Vector2 point)
    {
        if (points.Count > 0)
        {
            points[points.Count - 1] = point;
            UpdateLine();
        }
    }

    private void UpdateLine()
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0));
        }
    }

    public void ClearLine()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    public Color GetColor() => lineColor;
}