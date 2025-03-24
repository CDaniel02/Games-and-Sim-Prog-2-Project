using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button StartButton;
    public Button SettingsButton;
    public Button ExitButton;
    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(StartGame);
        SettingsButton.onClick.AddListener(OpenSettings);
        ExitButton.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartGame()
    {
        SceneManager.LoadScene("WorldScene");
    }

    private void OpenSettings()
    {

    }

    private void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
