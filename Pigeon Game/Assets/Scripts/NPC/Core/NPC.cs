using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

// renamed to just NPC NPC controller or something like that 

public class NPC : MonoBehaviour
{
    // name field that gets pulled from the hierarchuy

    
    public string Name;

    /*
    private Dictionary<string, Letter> _toMailbox; 
    public Dictionary<string, Letter> ToMailbox { get { return _toMailbox; } set { _toMailbox = value; } }

    private Dictionary<string, Letter> _fromMailbox; 
    public Dictionary<string, Letter> FromMailbox { get { return _fromMailbox; } set { _fromMailbox = value; } }
    */

    public Mailbox mailbox; 

    private Queue<string> _dialogQueue;

    private bool pigeonNearby = false;
    public Vector3 npcSpawnPoint;
    public Vector3 npcMovementEndPoint;
    public float walkingDuration = 3.0f;

    public void Start()
    {
        // ToMailbox = new Dictionary<string, Letter>();
        mailbox = new Mailbox(); 
        Name = GetComponent<CapsuleCollider>().name;

        Letter testLetter = new Letter("Naked Man", "Peasant");
        testLetter.FromResponse = "Bring this to the Naked Man at the lake post-haste"; 
        testLetter.ToResponse = "How DARE he say this about me?";
        if (Name == "Peasant")
        {
            mailbox.AddOutgoingMail(testLetter);
        }
        if (Name == "Naked Man")
        {
            Letter assisstantLetter = new Letter("Peasant", "Naked Man");
            assisstantLetter.FromResponse = "Bring this to him, he'll rue the day...";
            assisstantLetter.ToResponse = "What an imbicile he is...";
            //assisstantLetter.PrereqLetters.Add(testLetter); 
            mailbox.AddOutgoingMail(assisstantLetter);
        }
        
        
        // ToMailbox[testLetter.To] = testLetter;

        _dialogQueue = new Queue<string>();
        StartCoroutine(npcMove());
    }

    private IEnumerator npcMove()
    {
          while (pigeonNearby == false)
        {
            yield return null;
            //StartCoroutine(npcMoveToPoint(npcSpawnPoint, npcMovementEndpoint));
            //StartCoroutine(npcMoveToPoint(npcMovementEndpoint, npcSpawnPoint));
        }
    }
    private IEnumerator npcMoveToPoint()
    {
       yield return null;
    }

    public bool Interact(PlayerStateMachine pigeon)
    {
        DialogBox dialogBox = pigeon.dialogBox;

        if(_dialogQueue.Count > 0)
        {
            if(_dialogQueue.Peek() == "end")
            {
                _dialogQueue.Dequeue();
                dialogBox.dialogPanel.SetActive(false);
            }
            else
            {
                dialogBox.ShowDialog(_dialogQueue.Dequeue()); 
            }
        }
        else
        {
            Letter letterToReceive;
            if (pigeon.CheckAndGiveLetter(this, out letterToReceive))
            {
                mailbox.AddIncomingMail(letterToReceive);
                Debug.Log("Mail added"); 
                _dialogQueue.Enqueue("Thank you for the letter!");
                _dialogQueue.Enqueue(letterToReceive.ToResponse);
            }


            List<Letter> outgoingMail = mailbox.GetOutgoingMail();
            Debug.Log("Mail checked"); 
            Debug.Log(Name + " outgoing mail count: " + outgoingMail.Count); 
            if (outgoingMail.Count > 0)
            {
                foreach (Letter letter in outgoingMail)
                {
                    _dialogQueue.Enqueue("I have a letter to give you that goes to " + letter.To + ".");
                    _dialogQueue.Enqueue(letter.FromResponse);
                    _dialogQueue.Enqueue("*coo! I now have the letter that goes to " + letter.To + "*");
                    pigeon.Letters[letter.To] = letter;
                }

                _dialogQueue.Enqueue("end"); 
                Interact(pigeon);
            }
            else
            {
                _dialogQueue.Enqueue("I dont have any letters!");
                _dialogQueue.Enqueue("end");
                Interact(pigeon); 
            }
        }

        return dialogBox.dialogPanel.activeInHierarchy; 
        /*
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
                
                
       //     }
       //     else
       //     {
       //         dialogBox.ShowDialog("I dont have any letters!");
       //         foreach(KeyValuePair<string, Letter> letter in pigeon.Letters)
       //         {
       //             dialogBox.ShowDialog("coo! I now have the letter that goes to " + letter.Key);
       //         }
       //     }
            // check if npc has a letter
            // dialogbox letter from
            // give pigeon letter

            // dont have a letter, check if the pigeon has a letter that the npc wants


        //}
        
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
        
    }


}
