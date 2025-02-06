using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class NPC : MonoBehaviour
{
    // name field that gets pulled from the hierarchuy
    public string Name;

    // mailbox holds outgoing and incoming mail
    public Mailbox mailbox;
    private Queue<string> _dialogQueue;

    private bool pigeonNearby = false;
    //public NavMeshAgent agent;
    //public float range; //radius of sphere
    public Animator animator;

    [Header("Reference to NPC Movement Attributes")]
    public float npcMovementSpeed;
    public List<Transform> npcMovementPoints;

    public string ThankYouForLetter = "Thank you for the letter!"; 
    public string IHaveALetterTo = "I have a letter to give you that goes to ";
    public string IDontHaveAnyLetters = "I dont have any letters!"; 

    //public Transform centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    private void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("idle", true);
        mailbox = new Mailbox();
        Name = GetComponent<CapsuleCollider>().name;
        _dialogQueue = new Queue<string>();
        if (!pigeonNearby && npcMovementPoints != null)
        {
            npcMove(npcMovementPoints, npcMovementSpeed);
        }
    }

    private void Update()
    {


    }

    public void npcMove(List<Transform> pointList, float speed)
    {
        bool endOfRoute = false;
        int waypointIndex = 0;
        while (!endOfRoute)
        {
            transform.position = Vector3.MoveTowards(
            transform.position,
            pointList[waypointIndex].position,
            speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, pointList[waypointIndex].position) < 0.1f)
            {
                waypointIndex++;
                if (waypointIndex == pointList.Count - 1)
                {
                    endOfRoute = true;
                }
            }
        }
    }

    // function that gets called when the pigeon interacts with the NPC
    //
    // this mostly works by queueing dialog into the dialogQueue, then
    // running recursively to then read out that dialog to the player
    //
    // the player is meant to interact with the NPC multiple times until
    // all the dialog queued by the first interaction runs out 
    public bool Interact(PlayerStateMachine pigeon)
    {
        DialogBox dialogBox = pigeon.dialogBox;

        // we have already queued dialog
        if (_dialogQueue.Count > 0)
        {
            // "end" is the sign we've run to the end of the dialog, and to turn the panel off
            if (_dialogQueue.Peek() == "end")
            {
                _dialogQueue.Dequeue();
                dialogBox.dialogPanel.SetActive(false);
            }
            else // if its not end, we've got more dialog
            {
                dialogBox.ShowDialog(_dialogQueue.Dequeue());
            }
        }
        else // we have no dialog queued
        {
            Letter letterToReceive;

            // if the player has a letter, we will thank them and give out
            // the ToReponse 
            if (pigeon.CheckAndGiveLetter(this, out letterToReceive))
            {
                mailbox.AddIncomingMail(letterToReceive);
                _dialogQueue.Enqueue(ThankYouForLetter);
                _dialogQueue.Enqueue(letterToReceive.ToResponse);
            }

            // retreive all mail that might be given out
            // this is mail that has already validated all prereqs
            List<Letter> outgoingMail = mailbox.RemoveOutgoingMail();
            if (outgoingMail.Count > 0)
            {
                // if we have it we cycle through all avaiable letters
                // queueing up dialog for each
                // and then give the letter to the player 
                foreach (Letter letter in outgoingMail)
                {
                    _dialogQueue.Enqueue(IHaveALetterTo + letter.To + ".");
                    _dialogQueue.Enqueue(letter.FromResponse);
                    _dialogQueue.Enqueue("*coo! I now have the letter that goes to " + letter.To + "*");
                    pigeon.Letters[letter.To] = letter;
                    Debug.Log("Letter that goes to " + letter.To + " added to pigeon mailbox"); 
                }
            }
            else
            {
                // if we dont have any available mail, we simply
                // tell the player that
                _dialogQueue.Enqueue(IDontHaveAnyLetters);
            }

            // after everything, we should be done with dialog, and let the queue know that
            _dialogQueue.Enqueue("end");

            // run this function recursively to trigger the dialog to show
            Interact(pigeon);
        }

        return dialogBox.dialogPanel.activeInHierarchy;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Pigeon is nearby I cant move");
            pigeonNearby = true;
            animator.SetBool("idle", false);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Pigeon is gone I can move");
            pigeonNearby = false;
            animator.SetBool("idle", true);
        }
    }
    /*
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
*/
}
