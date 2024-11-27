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
    private int moveCount = 0;
    public int MoveCount => moveCount;

    private void Start()
    {
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();
    }

    public void StartLine(Dot dot, Vector2Int gridPosition)
    {
        // Check if the point is already connected
        if (dot.IsConnected())
        {
            RemoveLine(dot.GetDotColor());
            return;
        }

        activeDot = dot;
        lastGridPosition = gridPosition;
        linePath.Clear();
        linePath.Add(gridPosition);

        // If a line of this color already exists, delete it
        if (colorToLine.ContainsKey(dot.GetDotColor()))
        {
            RemoveLine(dot.GetDotColor());
        }

        // Create a new line
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
            // Deleting a line object
            Destroy(colorToLine[color].gameObject);
            colorToLine.Remove(color);

            // Releasing cells in the path of the line
            if (connectedDots.ContainsKey(color))
            {
                var (dot1, dot2) = connectedDots[color];
                dot1.SetConnected(false);
                dot2.SetConnected(false);
                connectedDots.Remove(color);
            }

            // Releasing all cells on the stored pathway
            if (colorToPaths.ContainsKey(color))
            {
                foreach (var pos in colorToPaths[color])
                {
                    var cell = gridManager.GetCell(pos.x, pos.y);
                    if (cell != null)
                    {
                        // Check if there is a dot or other line on the cell
                        var dotAtCell = gridManager.GetDotAtGridPosition(pos);
                        if (dotAtCell == null)
                        {
                            // Check if there is another line through this cell
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

        // Checking field boundaries
        if (newGridPosition.x < 0 || newGridPosition.x >= gridManager.Width ||
            newGridPosition.y < 0 || newGridPosition.y >= gridManager.Height)
            return;

        Vector2Int delta = newGridPosition - lastGridPosition;
        if (Mathf.Abs(delta.x) + Mathf.Abs(delta.y) != 1) return;

        // Reverse movement check
        if (linePath.Count > 1 && newGridPosition == linePath[linePath.Count - 2])
        {
            linePath.RemoveAt(linePath.Count - 1);
            lastGridPosition = newGridPosition;
            currentLine.RemoveLastPoint();
            return;
        }

        // Checking for the starting point
        if (newGridPosition == activeDot.GetGridPosition())
            return;

        // Checking for intersection with the current line
        if (linePath.Contains(newGridPosition))
            return;

        // Checking for point collision and cell occupancy
        Dot dotAtPosition = gridManager.GetDotAtGridPosition(newGridPosition);
        Cell cell = gridManager.GetCell(newGridPosition.x, newGridPosition.y);

        if (cell != null && cell.IsOccupied())
        {
            // Checking to see if it's a dot of the same color
            if (dotAtPosition == null || dotAtPosition.GetDotColor() != activeDot.GetDotColor())
                return;
        }

        if (dotAtPosition != null)
        {
            if (dotAtPosition.GetDotColor() != activeDot.GetDotColor())
                return; // Block movement if the point is of a different color

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
                moveCount++;
                activeDot.SetConnected(true);
                endDot.SetConnected(true);

                // Save a pair of connected points
                connectedDots[activeDot.GetDotColor()] = (activeDot, endDot);

                // Save the line path
                colorToPaths[activeDot.GetDotColor()] = new List<Vector2Int>(linePath);

                // Mark all squares along the way as occupied
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