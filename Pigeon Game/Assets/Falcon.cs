using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        angle = 0f;
        center = transform.position;
        pigeonSpotted = false;
        retreating = false;
        pursuitTimer = 0f;
    }

    void Update()
    {
        if(!retreating)
        {
            if (!pigeonSpotted)
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

                //if collision, endpursuit and send pigeon into stunned mode

                //calculate vector3 for pigeon to get sent in, draw a line between the two!
                //PlayerStateMachine.SwitchState(new StunnedState(Playerstatemachine));



                if (pursuitTime < pursuitTimer)
                {
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
        transform.position = orbitPosition;
    }

    void Pursue()
    {
        transform.position = Vector3.MoveTowards(transform.position, pigeon.position, pursuitSpeed);
        Debug.Log("The falcon is after you!");
    }

    void EndPursuit()
    {
        Debug.Log("You were too quick! The Falcon, exhausted from a failed hunt, retreats...");
        pursuitTimer = 0f;
        retreating = true;
        pigeonSpotted = false;
    }

    void Retreat()
    {
        transform.position = Vector3.MoveTowards(transform.position, orbitPosition, retreatSpeed);
        if (Vector3.Distance(transform.position, orbitPosition) < 0.1f)
        {
            retreating = false;
            Debug.Log("The falcon has returned home.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("There has been a collision! Uh oh."); //why isn't this working?!
        if(collision.otherCollider.gameObject == pigeon || collision.collider.gameObject == pigeon)
        {
            //pigeon.fall();
            Debug.Log("Got you! Haha.");
            EndPursuit();
        }
    }
}
