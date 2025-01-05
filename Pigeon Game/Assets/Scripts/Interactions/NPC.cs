using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// renamed to just NPC NPC controller or something like that 

public class NPC : MonoBehaviour
{
    // name field that gets pulled from the hierarchuy

    
    public string Name;

    private Dictionary<string, Letter> _toMailbox; 
    public Dictionary<string, Letter> ToMailbox { get { return _toMailbox; } set { _toMailbox = value; } }

    private Dictionary<string, Letter> _fromMailbox; 
    public Dictionary<string, Letter> FromMailbox { get { return _fromMailbox; } set { _fromMailbox = value; } }

    public void Start()
    {
        Name = GetComponent<CapsuleCollider>().name; 
    }

    public bool Interact(PlayerStateMachine pigeon)
    {
        DialogBox dialogBox = pigeon.dialogBox; 

        if (dialogBox.dialogPanel.activeInHierarchy)
        {
            dialogBox.dialogPanel.SetActive(false); 
        }
        else
        {
            dialogBox.ShowDialog("This is your first interaction");

            // check if npc has a letter
            // dialogbox letter from
            // give pigeon letter

            // dont have a letter, check if the pigeon has a letter that the npc wants

            
        }

        /*
        Letter letter;
        if(pigeon.CheckLetter(this, out letter))
        {
            letter souansdna
        }
        else
        {
            oops youve got nothin
        }
        */ 

        return dialogBox.dialogPanel.activeInHierarchy; 
    }
}
