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
        ToMailbox = new Dictionary<string, Letter>();
        Name = GetComponent<CapsuleCollider>().name;
        Letter testLetter = new Letter("The King's Assisstant", "The King");
        ToMailbox[testLetter.To] = testLetter;
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
            if(ToMailbox.Count > 0)
            {

                foreach(KeyValuePair<string, Letter> letter in ToMailbox)
                {
                    dialogBox.ShowDialog("I have a letter to give you that goes to " + letter.Key + ".");
                    pigeon.Letters[letter.Key] = letter.Value;
                    ToMailbox.Remove(letter.Key);
                }
                /*foreach(KeyValuePair<string, Letter> letter in pigeon.Letters)
                {
                    dialogBox.ShowDialog("coo! I now have the letter that goes to " + letter.Key);
                }
                */
                
                
            }
            else
            {
                dialogBox.ShowDialog("I dont have any letters!");
                foreach(KeyValuePair<string, Letter> letter in pigeon.Letters)
                {
                    dialogBox.ShowDialog("coo! I now have the letter that goes to " + letter.Key);
                }
            }
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
