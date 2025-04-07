using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool _paused = false;
    bool Paused // this is done this way so the game can be paused without having to invoke the notification
    {
        get
        {
            return _paused;
        }
        set
        {
            _paused = value;
            Time.timeScale = Paused ? 0f : 1f;
            PausePanel.SetActive(Paused);
        }
    }

    public GameObject PausePanel;

    void Start()
    {
        PausePanel.SetActive(false);

        NotificationCenter.Instance.AddObserver("Pause", Pause);
        NotificationCenter.Instance.AddObserver("ExitGame", ExitGame);
    }

    public void Pause(Notification notification)
    {
        Paused = !Paused;
    }

    public void ExitGame(Notification notification)
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
