using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button ResumeButton;
    public Button SettingsButton;
    public Button ExitButton;
    // Start is called before the first frame update
    void Start()
    {
        ResumeButton.onClick.AddListener(ResumeGame());
        SettingsButton.onClick.AddListener(OpenSettings);
        ExitButton.onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
