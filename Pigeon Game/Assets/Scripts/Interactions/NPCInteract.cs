using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// renamed to just NPC NPC controller or something like that 

public class NPCInteract : MonoBehaviour
{
    public DialogBox dialogBox;

    public bool Interact()
    {
        if (dialogBox.dialogPanel.activeInHierarchy)
        {
            dialogBox.dialogPanel.SetActive(false); 
        }
        else
        {
            dialogBox.ShowDialog("This is your first interaction"); 
        }

        return dialogBox.dialogPanel.activeInHierarchy; 
    }
}
