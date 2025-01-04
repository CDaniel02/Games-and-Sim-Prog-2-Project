using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem; 

public class Player : MonoBehaviour {

	private Animator anim;
	// private CharacterController controller;
	public Rigidbody rb; 

	public float speed = 30.0f;
	//public float turnSpeed = 400.0f;
	private Vector3 moveDirection = Vector3.zero;
	//public float gravity = 20.0f;

	void Start ()
	{
		rb = GetComponent <Rigidbody>();
		anim = gameObject.GetComponentInChildren<Animator>();
	}

	void Update ()
	{
		// set the rigidbody velocity to what we've got from the input 	
		rb.velocity = new UnityEngine.Vector3(rb.velocity.x, rb.velocity.y, moveDirection.z * speed);

		// animations
		if(moveDirection.z > 0)
		{
            anim.SetInteger("AnimationPar", 1);
        }
		else
		{
            anim.SetInteger("AnimationPar", 0);
        }

		/*
		if(controller.isGrounded){
			moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
		}
		*/

		//float turn = Input.GetAxis("Horizontal");
		//transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
		//controller.Move(moveDirection * Time.deltaTime);
		//moveDirection.y -= gravity * Time.deltaTime;
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
