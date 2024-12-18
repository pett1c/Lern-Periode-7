using UnityEngine;

public class GameCompletionChecker : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LineManager lineManager;
    [SerializeField] private GameObject victoryPanel;

    private void Start()
    {
        if (lineManager != null)
            lineManager.OnLineCompleted += CheckGameCompletion;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
            // ����������, ��� VictoryPanelManager ������� ���������� ������
            var victoryManager = victoryPanel.GetComponent<VictoryPanelManager>();
            if (victoryManager != null)
            {
                victoryManager.Initialize(gridManager, lineManager);
            }
        }
    }

    private void OnDestroy()
    {
        if (lineManager != null)
            lineManager.OnLineCompleted -= CheckGameCompletion;
    }

    private void CheckGameCompletion()
    {
        if (gridManager.IsGridFull() && gridManager.AreAllDotsConnected())
        {
            ShowVictory();
        }
    }

    private void ShowVictory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            // ��������� ������ � VictoryPanelManager ��� ������ ������
            var victoryManager = victoryPanel.GetComponent<VictoryPanelManager>();
            if (victoryManager != null)
            {
                victoryManager.UpdatePerfectionScore();
            }
        }
    }
}