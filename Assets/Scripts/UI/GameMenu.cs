using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private Button CallMenuButton;
    [SerializeField]
    private GameObject GameMenuPanel;
    [SerializeField]
    private Button ContinueButton;
    [SerializeField]
    private Button ExitButton;

    private void Awake()
    {
        ContinueButton.onClick.AddListener(HideMenu);
        CallMenuButton.onClick.AddListener(ShowMenu);
        ExitButton.onClick.AddListener(ExitGame);
        HideMenu();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGameMenu();
        }
    }

    private void ToggleGameMenu()
    {
        if(GameMenuPanel.activeSelf)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void ShowMenu()
    {
        GameMenuPanel.SetActive(true);
        CallMenuButton.gameObject.SetActive(false);
    }

    private void HideMenu()
    {
        GameMenuPanel.SetActive(false);
        CallMenuButton.gameObject.SetActive(true);
    }
}
