using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public GameObject dialogPanel;
    public Text dialogText;

    void Start()
    {
        // Hide the dialog box initially
        dialogPanel.SetActive(false);

    }

    public void ShowDialog(string message)
    {
        dialogText.text = message;
        dialogPanel.SetActive(true);
    }
}