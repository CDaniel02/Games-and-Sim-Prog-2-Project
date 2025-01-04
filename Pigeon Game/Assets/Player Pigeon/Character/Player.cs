using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem; 

public class Player : MonoBehaviour {

	private Animator anim;
	public Rigidbody rb; 

	public float speed = 30.0f;
	public float turnSpeed = 400.0f;
	private Vector3 moveDirection = Vector3.zero;

	void Start ()
	{
		rb = GetComponent <Rigidbody>();
		anim = gameObject.GetComponentInChildren<Animator>();
	}

	void Update ()
	{
		// set the rigidbody velocity to what we've got from the input 	
		rb.velocity = new UnityEngine.Vector3(rb.velocity.x, rb.velocity.y, moveDirection.z * speed);

		// rotating the pigeon
		transform.Rotate(0, moveDirection.x * turnSpeed * Time.deltaTime, 0); 

		// animations
		if(moveDirection.z > 0)
		{
            anim.SetInteger("AnimationPar", 1);
        }
		else
		{
            anim.SetInteger("AnimationPar", 0);
        }
	}

	// every move input we set the MoveDirection to the input value inputted
	void OnMove(InputValue inputValue)
	{
		moveDirection = new UnityEngine.Vector3(inputValue.Get<UnityEngine.Vector2>().x, rb.velocity.y, inputValue.Get<UnityEngine.Vector2>().y); 
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
		{
			Debug.Log("we hit the ground guys"); 
		}
    }
}
