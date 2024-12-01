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
        // ����������� ������ �������� � ����
        if (menuButton != null)
        {
            menuButton.onClick.AddListener(ReturnToMenu);
        }

        // ����������� ������ ���������� ������
        if (nextLevelButton != null)
        {
            // ���������, ���������� �� ��������� �������
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName.StartsWith("Level"))
            {
                // ��������� ����� �������� ������
                if (int.TryParse(currentSceneName.Substring(5), out int currentLevel))
                {
                    string nextLevelName = $"Level{currentLevel + 1}";

                    // ���������, ���������� �� ����� ���������� ������
                    if (SceneExists(nextLevelName))
                    {
                        nextLevelButton.onClick.AddListener(() => LoadNextLevel(nextLevelName));
                    }
                    else
                    {
                        // ���� ���������� ������ ���, �������� ������
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
        // ��������� ������� ����� � �����
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