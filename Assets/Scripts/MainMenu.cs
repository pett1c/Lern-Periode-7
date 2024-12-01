using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private List<Button> levelButtons = new List<Button>();

    [Header("Secret Elements")]
    [SerializeField] private Button threeDotsButton;
    [SerializeField] private TextMeshProUGUI secretText;
    [SerializeField] private TextMeshProUGUI clickCounterText;

    private int dotsClickCount = 0;
    private const int REQUIRED_CLICKS = 25;
    private const int MAX_CLICKS = 100;

    private void Start()
    {
        SetupButtons();
        SetupSecretButton();
        InitializeUI();
    }

    private void InitializeUI()
    {
        // ������������� �������� ������
        if (clickCounterText != null)
        {
            clickCounterText.text = "0";
        }

        // ������������� ���������� ������
        if (secretText != null)
        {
            secretText.text = "Three Puntos, amigo!";
            secretText.color = Color.white;
            secretText.gameObject.SetActive(false);
        }
    }

    private void SetupButtons()
    {
        // ����������� ������ ������ � ������
        for (int i = 0; i < levelButtons.Count; i++)
        {
            int levelNumber = i + 1;
            Button button = levelButtons[i];

            if (button != null)
            {
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = levelNumber.ToString();
                }

                button.onClick.AddListener(() => LoadLevel(levelNumber));
            }
        }
    }

    private void SetupSecretButton()
    {
        if (threeDotsButton != null)
        {
            threeDotsButton.onClick.AddListener(OnThreeDotsClick);
        }
    }

    private void OnThreeDotsClick()
    {
        if (dotsClickCount < MAX_CLICKS)
        {
            dotsClickCount++;

            // ��������� ����� ��������
            if (clickCounterText != null)
            {
                clickCounterText.text = dotsClickCount.ToString();
            }

            // ��������� ������� ��� ���������� ������
            if (dotsClickCount == REQUIRED_CLICKS && secretText != null)
            {
                secretText.gameObject.SetActive(true);
            }
        }
    }

    private void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene($"Level{levelNumber}");
    }
}