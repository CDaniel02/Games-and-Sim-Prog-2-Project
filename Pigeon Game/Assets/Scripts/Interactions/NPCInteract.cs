using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public DialogBox dialogBox;
    public void Interact()
    {
        if (dialogBox.dialogPanel.activeInHierarchy)
        {
            dialogBox.dialogPanel.SetActive(false);
        }
        else
        {
            dialogBox.ShowDialog("This is your first interaction");
        }
        
    }
}
