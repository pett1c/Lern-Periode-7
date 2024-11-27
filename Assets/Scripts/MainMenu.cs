using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;

    private void Start()
    {
        // Customizing the level buttons
        SetupButton(level1Button, "1", 1);
        SetupButton(level2Button, "2", 2);
        SetupButton(level3Button, "3", 3);
    }

    private void SetupButton(Button button, string text, int levelNumber)
    {
        if (button != null)
        {
            // Set the button text
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
                buttonText.text = text;

            // Add a click handler
            button.onClick.AddListener(() => LoadLevel(levelNumber));
        }
    }

    private void LoadLevel(int levelNumber)
    {
        // Load the scene with the appropriate level
        SceneManager.LoadScene($"Level{levelNumber}");
    }
}