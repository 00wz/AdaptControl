using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button StartButton;
    [SerializeField]
    private Button ExitButton;

    private void Awake()
    {
        StartButton.onClick.AddListener(LoadFirstScene);
        ExitButton.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }
}
