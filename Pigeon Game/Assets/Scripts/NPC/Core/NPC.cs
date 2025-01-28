using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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
    public NavMeshAgent agent;
    public float range; //radius of sphere
    public Animator animator;

    public Transform centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("idle", true);
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

    }

    private void Update()
    {
        if (!pigeonNearby)
        {
            Move();
        }
        
    }

    public void Move()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            animator.SetBool("idle", true);
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
                animator.SetBool("idle", false);
            }
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public bool Interact(PlayerStateMachine pigeon)
    {
        DialogBox dialogBox = pigeon.dialogBox;

        if (_dialogQueue.Count > 0)
        {
            if (_dialogQueue.Peek() == "end")
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Pigeon is nearby I cant move");
            pigeonNearby = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Pigeon is gone I can move");
            pigeonNearby = false;
        }
    }


}
