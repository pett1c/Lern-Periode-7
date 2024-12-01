using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryPanelManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button menuButton;
    [SerializeField] private Button nextLevelButton;

    private void Start()
    {
        // Настраиваем кнопку возврата в меню
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ReturnToMenu);
        }

        // Настраиваем кнопку следующего уровня
        if (nextLevelButton != null)
        {
            // Проверяем, существует ли следующий уровень
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName.StartsWith("Level"))
            {
                // Извлекаем номер текущего уровня
                if (int.TryParse(currentSceneName.Substring(5), out int currentLevel))
                {
                    string nextLevelName = $"Level{currentLevel + 1}";

                    // Проверяем, существует ли сцена следующего уровня
                    if (SceneExists(nextLevelName))
                    {
                        nextLevelButton.onClick.AddListener(() => LoadNextLevel(nextLevelName));
                    }
                    else
                    {
                        // Если следующего уровня нет, скрываем кнопку
                        nextLevelButton.gameObject.SetActive(false);
                    }
                }
            }
        }
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
        // Проверяем наличие сцены в билде
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