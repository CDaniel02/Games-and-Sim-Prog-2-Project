using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button ResumeButton;
    public Button SettingsButton;
    public Button ExitButton;

    public GameObject PausePanel;

    public bool paused = false; 
    
    void Start()
    {
        PausePanel.SetActive(false);
        ResumeButton.onClick.AddListener(Pause);
        SettingsButton.onClick.AddListener(OpenSettings);
        ExitButton.onClick.AddListener(ExitGame);

        NotificationCenter.Instance.AddObserver("Pause", Pause); 
    }

    void Update()
    {

    }

    public void ResumeGame()
    {
        if(PausePanel != null)
        {
            PausePanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }

    public void Pause(Notification notification)
    {
        Pause(); 
    }

    public void Pause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
        PausePanel.SetActive(paused);
    }

    public void OpenSettings()
    {

    }

    private void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
