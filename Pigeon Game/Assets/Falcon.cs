using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falcon : MonoBehaviour
{
    public float speed = 1f;
    public float radius = 20f;
    private float angle;
    private Vector3 center;

    public float detectedable = 20f;
    private bool pigeonSpotted;

    void Start()
    {
        angle = 0f;
        center = transform.position;
        pigeonSpotted = false;
    }

    void Update()
    {
        //increment angle
        angle += speed * Time.deltaTime;

        //update position
        transform.position = new Vector3((center.x + Mathf.Cos(angle) * radius), transform.position.y, (center.z + Mathf.Sin(angle) * radius));


        /**
        if (Vector3.Distance(transform.position, player.position) <= detectedable)
        {
            if (!pigeonSpotted)
            {
                pigeonSpotted = true;
            }
            Pursue();
        }
        */


    }

    void Pursue()
    {

    }


}
