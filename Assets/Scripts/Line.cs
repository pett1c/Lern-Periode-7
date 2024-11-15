using UnityEngine;
using System.Collections.Generic;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private Color lineColor;
    private List<Vector2> points = new List<Vector2>();

    private void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetColor(Color color)
    {
        lineColor = color;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    public void AddPoint(Vector2 point)
    {
        points.Add(point);
        UpdateLine();
    }

    public void RemoveLastPoint()
    {
        if (points.Count > 0)
        {
            points.RemoveAt(points.Count - 1);
            UpdateLine();
        }
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
            lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, -0.2f));
        }
    }

    public void ClearLine()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    public Color GetColor()
    {
        return lineColor;
    }

    public bool ContainsPoint(Vector2 point)
    {
        return points.Contains(point);
    }
}