using System;
using System.Collections;
using UnityEngine;

public abstract class State
{
	public abstract void Enter();
	public abstract void Update();
	public abstract void Exit();
}

public abstract class PlayerBaseState : State
{
    protected readonly PlayerStateMachine stateMachine;

    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected UnityEngine.Vector2 CalculateMoveDirection()
    {
        Vector3 cameraForward = new(stateMachine.MainCamera.forward.x, 0, stateMachine.MainCamera.forward.z);
        Vector3 cameraRight = new(stateMachine.MainCamera.right.x, 0, stateMachine.MainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * stateMachine.InputReader.MoveComposite.y + cameraRight.normalized * stateMachine.InputReader.MoveComposite.x;

        stateMachine.Velocity.x = moveDirection.x * stateMachine.MovementSpeed;
        stateMachine.Velocity.z = moveDirection.z * stateMachine.MovementSpeed;

        return stateMachine.Velocity; 
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
        if (stateMachine.Velocity.y > Physics.gravity.y)
        {
        }
            stateMachine.Velocity.y += Physics.gravity.y * Time.deltaTime;
    }

    protected void Move()
    {
        stateMachine.Controller.Move(stateMachine.Velocity * Time.deltaTime);
    }
}

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("GroundedState entered"); 
        stateMachine.Velocity.y = Physics.gravity.y;
        stateMachine.InputReader.OnJumpPerformed += SwitchToAirborneState;
    }

    public override void Update()
    {
        if (!stateMachine.Controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerAirborneState(stateMachine));
        }

        UnityEngine.Vector2 playerMovement = CalculateMoveDirection();
        // if playermovent is > 0, we play hop, otherwise, we play idle
        stateMachine.Animator.SetFloat("GroundMovementSpeed", Mathf.Abs(playerMovement.x) + Math.Abs(playerMovement.y)); 
        FaceMoveDirection();
        Move();

        // stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.MoveComposite.sqrMagnitude > 0f ? 1f : 0f, AnimationDampTime, Time.deltaTime);
    }

    public override void Exit()
    {
        Debug.Log("Groundedstate exited");
        stateMachine.InputReader.OnJumpPerformedClear(); //-= SwitchToAirborneState;
    }

    private void SwitchToAirborneState()
    {
        stateMachine.Animator.SetTrigger("Jumped"); 
        stateMachine.SwitchState(new PlayerAirborneState(stateMachine));
    }
}

public class PlayerAirborneState : PlayerBaseState
{
    private bool _canFlap;
    private float _flapCoolDown = 0.95f; 

    public PlayerAirborneState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        _canFlap = true; 
    }

    public override void Enter()
    {
        Debug.Log("Airborne State entered"); 
        stateMachine.Velocity.y = Physics.gravity.y; // half gravity? in air so that the pigeon falls down slowly 
        stateMachine.InputReader.OnJumpPerformed += FlapWings;

        // flap wings upon entering 
        FlapWings(); 
        _canFlap = true; 
    }

    public override void Update()
    {
        ApplyGravity(); 

        UnityEngine.Vector2 playerMovement = CalculateMoveDirection();
        stateMachine.Animator.SetFloat("AirMovementSpeed", Mathf.Abs(playerMovement.x) + Math.Abs(playerMovement.y)); 
        FaceMoveDirection();
        Move();

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

    private void FlapWings()
    {
        if(_canFlap)
        {
            stateMachine.Animator.SetTrigger("Jumped");
            Debug.Log("Flapping wings");
            // increase y velocity
            stateMachine.Velocity.y += stateMachine.FlapForce;
            // new Vector3(stateMachine.Velocity.x, stateMachine.FlapForce, stateMachine.Velocity.z);
            // if moving forward, play one flap animation

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
}