using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public abstract class State
{
	public abstract void Enter();
	public abstract void Update();
	public abstract void Exit();
    public abstract void FixedUpdate(); 
}

public abstract class PlayerBaseState : State
{
    // TODO: sometimes the first jump just doesnt work ?
    // TODO: make a and d sway side to side

    protected readonly PlayerStateMachine stateMachine;
    protected bool _canFlap;
    protected float _flapCoolDown = 0.95f;

    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

        _canFlap = true; 

        stateMachine.InputReader.OnInteractPerformed = Interact; 
    }

    protected UnityEngine.Vector3 CalculateMoveDirection()
    {
        Vector3 cameraForward = new(stateMachine.MainCamera.forward.x, 0, stateMachine.MainCamera.forward.z);
        Vector3 cameraRight = new(stateMachine.MainCamera.right.x, 0, stateMachine.MainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * stateMachine.InputReader.MoveComposite.y + cameraRight.normalized * stateMachine.InputReader.MoveComposite.x;

        return moveDirection; 
    }

    protected UnityEngine.Vector3 CalculateWalking()
    {
        Vector3 moveDirection = CalculateMoveDirection(); 

        stateMachine.Velocity.x = moveDirection.x * stateMachine.GroundedMovementSpeed;
        stateMachine.Velocity.z = moveDirection.z * stateMachine.GroundedMovementSpeed;

        return stateMachine.Velocity;
    }

    protected UnityEngine.Vector3 CalculateFlying()
    {
        // this code is pretty messed up but this took me so long lol
        Vector3 cameraForward = new(stateMachine.MainCamera.forward.x, 0, stateMachine.MainCamera.forward.z);
        Vector3 cameraRight = new(stateMachine.MainCamera.right.x, 0, stateMachine.MainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * stateMachine.InputReader.MoveComposite.y + cameraRight.normalized * stateMachine.InputReader.MoveComposite.x;

        Vector2 horizontalVelocity = new(stateMachine.Velocity.x, stateMachine.Velocity.z);

        horizontalVelocity.x += moveDirection.x * (stateMachine.AirborneMovementSpeed * Time.deltaTime);
        horizontalVelocity.y += moveDirection.z * (stateMachine.AirborneMovementSpeed * Time.deltaTime);

        Vector2 forward = new(stateMachine.MainCamera.forward.x, stateMachine.MainCamera.forward.z);

        Quaternion rotation = Quaternion.FromToRotation(horizontalVelocity, forward);
        Vector2 rotatedFrom = rotation * horizontalVelocity;

        stateMachine.Velocity.x = rotatedFrom.x;
        stateMachine.Velocity.z = rotatedFrom.y;

        CalculateDrag();

        FaceMoveDirection();

        Move(); 

        return stateMachine.Velocity;
    }

    protected void CalculateDrag()
    {
        stateMachine.Velocity *= (1 - Time.deltaTime * stateMachine.Drag);
    }

    protected void FaceMoveDirection()
    {
        Vector3 faceDirection = new(stateMachine.Velocity.x, 0f, stateMachine.Velocity.z);

        if (faceDirection == Vector3.zero)
            return;

        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.LookRotationDampFactor * Time.deltaTime);
    }

    protected void ApplyGravity()
    {
        stateMachine.Velocity.y += Physics.gravity.y * Time.deltaTime;
    }

    protected void Move()
    {
        stateMachine.Controller.Move(stateMachine.Velocity * Time.deltaTime);
    }

    protected void Interact()
    {
        Collider[] colliderArray = Physics.OverlapSphere(stateMachine.gameObject.transform.position, stateMachine.interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPC npc))
            {
                bool active = npc.Interact(stateMachine);
                if (active)
                {
                    stateMachine.SwitchActionMap("Dialog");
                    // TODO: freeze bird when talking bc it would be cool (and prevent soltlocking) 
                }
                else
                {
                    stateMachine.SwitchActionMap("Player");
                }
            }
        }
    }

    protected void FlapWings()
    {
        if (_canFlap)
        {
            stateMachine.Animator.SetTrigger("Jumped");
            Debug.Log("Flapping wings");
            stateMachine.Velocity.y += stateMachine.FlapForce;

            _canFlap = false;

            IEnumerator validateFlapCoroutine = ValidateFlap();
            stateMachine.StartCoroutine(validateFlapCoroutine);
        }

    }

    private IEnumerator ValidateFlap()
    {
        yield return new WaitForSeconds(_flapCoolDown);
        _canFlap = true;
    }

    public override void FixedUpdate()
    {

    }
}

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("GroundedState entered"); 
        // stateMachine.Velocity.y = Physics.gravity.y;
        stateMachine.InputReader.OnJumpPerformed += FlapWings;
    }

    public override void Update()
    {
        if (!stateMachine.Controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerAirborneState(stateMachine));
        }

        UnityEngine.Vector3 playerMovement = CalculateWalking(); 
        stateMachine.Animator.SetFloat("GroundMovementSpeed", Mathf.Abs(Mathf.Floor(playerMovement.x) + Mathf.Abs(Mathf.Floor(playerMovement.z)))); 
        FaceMoveDirection();
        Move();

        // stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.MoveComposite.sqrMagnitude > 0f ? 1f : 0f, AnimationDampTime, Time.deltaTime);
    }

    public override void Exit()
    {
        Debug.Log("Groundedstate exited");
        stateMachine.InputReader.OnJumpPerformedClear(); //-= SwitchToAirborneState;
    }
}

public class PlayerAirborneState : PlayerBaseState
{
    public PlayerAirborneState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        _canFlap = true; 
    }

    public override void Enter()
    {
        stateMachine.Animator.SetTrigger("Jumped");
        Debug.Log("Airborne State entered"); 
        // stateMachine.Velocity.y = Physics.gravity.y; // half gravity? in air so that the pigeon falls down slowly 
        stateMachine.InputReader.OnJumpPerformed += FlapWings;

        _canFlap = true; 
    }

    public override void Update()
    {
        ApplyGravity();

        UnityEngine.Vector3 playerMovement = CalculateFlying(); 
        stateMachine.Animator.SetFloat("AirMovementSpeed", Mathf.Abs(Mathf.Floor(playerMovement.x) + Mathf.Abs(Mathf.Floor(playerMovement.z)))); 


        if (stateMachine.Controller.isGrounded)
        {
            stateMachine.Animator.SetTrigger("Landed");
            Debug.Log("Player landed"); 
            stateMachine.SwitchState(new PlayerGroundedState(stateMachine));
        }

        // if playermovent is 0, we play idle flapping animation
        // if playermovent is > 0, we play gliding animation

        // stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.MoveComposite.sqrMagnitude > 0f ? 1f : 0f, AnimationDampTime, Time.deltaTime);
    }

    public override void Exit()
    {
        Debug.Log("Airbornestate exited"); 
        stateMachine.InputReader.OnJumpPerformed -= FlapWings;
    }
}