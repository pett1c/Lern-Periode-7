using UnityEngine;
using TMPro;

public class MoveCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveText;
    [SerializeField] private LineManager lineManager;

    private void Start()
    {
        if (lineManager == null)
            lineManager = FindObjectOfType<LineManager>();

        lineManager.OnLineCompleted += UpdateMoveCounter;
        UpdateMoveDisplay(0);
    }

    private void OnDestroy()
    {
        if (lineManager != null)
            lineManager.OnLineCompleted -= UpdateMoveCounter;
    }

    private void UpdateMoveCounter()
    {
        UpdateMoveDisplay(lineManager.MoveCount);
    }

    private void UpdateMoveDisplay(int moves)
    {
        if (moveText != null)
            moveText.text = $"Moves: {moves}";
    }
}