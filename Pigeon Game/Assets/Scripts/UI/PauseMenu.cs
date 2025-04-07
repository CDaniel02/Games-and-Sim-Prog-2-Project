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
    
    void Start()
    {
        ResumeButton.onClick.AddListener(Pause);
        SettingsButton.onClick.AddListener(OpenSettings);
        ExitButton.onClick.AddListener(ExitGame);
    }

    /*
    public void ResumeGame()
    {
        if(PausePanel != null)
        {
            PausePanel.SetActive(false);
        }
        Time.timeScale = 1f;
    }
    */

    public void Pause()
    {
        Notification notification = new("Pause", this);
        NotificationCenter.Instance.PostNotification(notification);
    }

    public void OpenSettings()
    {

    }

    private void ExitGame()
    {
        Notification notification = new("ExitGame", this);
        NotificationCenter.Instance.PostNotification(notification);
    }
}
