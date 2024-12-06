using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryPanelManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button menuButton;
    [SerializeField] private Button nextLevelButton;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI perfectionScoreText;

    private LineManager lineManager;
    private GridManager gridManager;

    private void Start()
    {
        SetupButtons();
    }

    public void Initialize(GridManager grid, LineManager line)
    {
        gridManager = grid;
        lineManager = line;
    }

    private void SetupButtons()
    {
        if (menuButton != null)
            menuButton.onClick.AddListener(ReturnToMenu);

        if (nextLevelButton != null)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName.StartsWith("Level"))
            {
                if (int.TryParse(currentSceneName.Substring(5), out int currentLevel))
                {
                    string nextLevelName = $"Level{currentLevel + 1}";
                    if (SceneExists(nextLevelName))
                    {
                        nextLevelButton.onClick.AddListener(() => LoadNextLevel(nextLevelName));
                    }
                    else
                    {
                        nextLevelButton.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void UpdatePerfectionScore()
    {
        if (perfectionScoreText != null && lineManager != null && gridManager != null)
        {
            float perfectionScore = CalculatePerfectionScore();
            perfectionScoreText.text = $"{Mathf.RoundToInt(perfectionScore)}%";
        }
    }

    private float CalculatePerfectionScore()
    {
        if (lineManager == null || gridManager == null)
        {
            Debug.LogError("LineManager or GridManager is missing!");
            return 0f;
        }

        int perfectMoves = gridManager.GetDotPairsCount();
        int playerMoves = lineManager.MoveCount;

        if (playerMoves <= 0)
        {
            Debug.LogWarning("Player moves is zero or negative!");
            return 0f;
        }

        if (perfectMoves <= 0)
        {
            Debug.LogWarning("No dot pairs found in the grid!");
            return 0f;
        }

        float perfectionScore = Mathf.Min(100f, (perfectMoves / (float)playerMoves) * 100f);

        return perfectionScore;
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void LoadNextLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string scene = System.IO.Path.GetFileNameWithoutExtension(path);
            if (scene == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}