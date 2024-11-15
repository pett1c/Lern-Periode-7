using UnityEngine;

public class GameCompletionChecker : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LineManager lineManager;
    [SerializeField] private GameObject victoryPanel; // Панель с сообщением о победе

    private void Start()
    {
        if (lineManager != null)
            lineManager.OnLineCompleted += CheckGameCompletion;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
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
            // Можно добавить звуковой эффект, анимацию или другие эффекты победы
        }
        Debug.Log("Уровень пройден!");
    }
}