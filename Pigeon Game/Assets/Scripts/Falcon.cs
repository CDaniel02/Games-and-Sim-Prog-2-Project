using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Falcon : MonoBehaviour
{
    //falcon speeds for each behavior
    public float hoverSpeed = 0.3f;
    public float pursuitSpeed = 0.1f;
    public float retreatSpeed = 0.07f;

    private float angle; //angle of orbit
    public float radius = 50f; //radius of orbit
    private Vector3 center; //center of orbit
    private Vector3 orbitPosition; //most recent position in orbit

    public float detectedable = 30f; //range pigeon is detected within
    private bool pigeonSpotted; //if pigeon is within range
    private bool retreating; //if falcon is retreating from pursuit

    public Transform pigeon; //pigeon (player)

    private float pursuitTimer; //tracks pursuit time
    public float pursuitTime = 20f; //time pursuit lasts for

    private float bufferTimer;
    public float bufferTime = 10f;
    private bool buffering;

    private PlayerStateMachine playerStateMachine;

    void Start()
    {
        angle = 0f;
        center = transform.position;
        pigeonSpotted = false;
        retreating = false;
        pursuitTimer = 0f;
        bufferTimer = 0f;
        buffering = false;
    }

    void Update()
    {
        if (!retreating)
        {
            if (buffering)
            {
                Hover();
                bufferTimer += Time.deltaTime;
                if (bufferTime < bufferTimer)
                {
                    EndBuffer();
                }
            }
            else if (!pigeonSpotted)
            {
                Hover();
                if (Vector3.Distance(transform.position, pigeon.position) <= detectedable)
                {
                    pigeonSpotted = true;
                    Debug.Log("Oh no! You've been spotted!");
                }
            }
            else if (pigeonSpotted)
            {
                Pursue();
                pursuitTimer += Time.deltaTime;

                if (pursuitTime < pursuitTimer)
                {
                    Debug.Log("You were too quick! The Falcon, exhausted from a failed hunt, retreats...");
                    EndPursuit();
                }
            }
        }
        else
        {
            Retreat();
        }
    }

    void Hover()
    {
        //increment angle
        angle += hoverSpeed * Time.deltaTime;

        //update position
        orbitPosition = new Vector3((center.x + Mathf.Cos(angle) * radius), transform.position.y, (center.z + Mathf.Sin(angle) * radius));

        //rotate to face orbit direction
        Vector3 moveDirection = (orbitPosition - transform.position).normalized;
        if (moveDirection.magnitude > 0.01f)
        {
            transform.forward = moveDirection; //rotates to face direction
        }

        transform.position = orbitPosition;
    }
    void Pursue()
    {
        Vector3 moveDirection = (pigeon.position - transform.position).normalized;

        //rotate to face pigeon
        if (moveDirection.magnitude > 0.01f)
        {
            transform.forward = moveDirection;
        }

        transform.position = Vector3.MoveTowards(transform.position, pigeon.position, pursuitSpeed);
        //Debug.Log("The falcon is after you!");
    }

    void Retreat()
    {
        Vector3 moveDirection = (orbitPosition - transform.position).normalized;

        //rotate to face home
        if (moveDirection.magnitude > 0.01f)
        {
            transform.forward = moveDirection;
        }

        transform.position = Vector3.MoveTowards(transform.position, orbitPosition, retreatSpeed);
        if (Vector3.Distance(transform.position, orbitPosition) < 0.1f)
        {
            retreating = false;
            buffering = true;
            Debug.Log("The falcon has returned home.");
        }
    }

    void EndPursuit()
    {
        Debug.Log("Pursuit terminating.");
        pursuitTimer = 0f;
        retreating = true;
        pigeonSpotted = false;
    }

    void EndBuffer()
    {
        bufferTimer = 0f;
        buffering = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("There has been a collision! Uh oh.");
        if(collision.gameObject.name == "Pigeon")
        {
            //calculate vector3 for pigeon to get sent in, draw a line between the two!
            Vector3 knockBack = (collision.transform.position - transform.position).normalized;
            knockBack.y = -0.5f;

            playerStateMachine = collision.gameObject.GetComponent<PlayerStateMachine>();
            playerStateMachine.SwitchState(new StunnedState(playerStateMachine, knockBack));
            Debug.Log("Got you! Haha.");
            EndPursuit();
        }
    }
}
