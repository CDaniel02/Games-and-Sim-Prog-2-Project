using System;
using System.Collections;
//using UnityEditor.Rendering;
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

        stateMachine.InputReader.OnUnlockCursorPerformed = UnlockCurser;
        stateMachine.InputReader.OnClickPerformed = ClickPerformed;
        stateMachine.InputReader.OnDialogPerformed = Dialog;
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
        Vector2 cameraForward = new(stateMachine.MainCamera.forward.x, stateMachine.MainCamera.forward.z);
        Vector2 forward = cameraForward; 
        Vector2 cameraRight = new(stateMachine.MainCamera.right.x, stateMachine.MainCamera.right.z);

        Vector2 moveDirection = forward.normalized * stateMachine.InputReader.MoveComposite.y; // + cameraRight.normalized * stateMachine.InputReader.MoveComposite.x;

        Vector2 horizontalVelocity = new(stateMachine.Velocity.x, stateMachine.Velocity.z);

        horizontalVelocity += moveDirection * (stateMachine.AirborneMovementSpeed * Time.deltaTime);

        stateMachine.Animator.SetFloat("Tilt", stateMachine.InputReader.MoveComposite.x); 
        if(stateMachine.InputReader.MoveComposite.x != 0)
        {

            Vector2 angle = cameraRight.normalized * stateMachine.InputReader.MoveComposite.x;
            angle += forward;

            Quaternion angleRotation = Quaternion.FromToRotation(forward, angle);
            Vector2 newForward = angleRotation * forward;

            float step = stateMachine.AirborneMovementSpeed * stateMachine.RotationalFactor * Time.deltaTime;
            forward = Vector2.MoveTowards(forward, newForward, step);
        }


        Quaternion rotation = Quaternion.FromToRotation(horizontalVelocity, forward);
        // rotation = rotation + stateMachine.InputReader.MoveComposite.x * 45;
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

    protected void UnlockCurser()
    {
        stateMachine.CurserLocked = !stateMachine.CurserLocked;
        // stateMachine.CinemachineCamera.enabled = stateMachine.CurserLocked; 
    }

    protected void ClickPerformed()
    {
        if(stateMachine.InputReader.MouseDown)
        {
            IEnumerator drag = CheckIfDrag(); 
            stateMachine.StartCoroutine(drag);
        }
        else
        {
            ClickRelease(); 
        }

        /*
        if (stateMachine.InputReader.MouseDown)
        {
            LayerMask layerMask = stateMachine.UICamera.cullingMask;

            Vector3 mousePos = stateMachine.InputReader.MousePosition;
            Ray ray = stateMachine.UICamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                GameObject letter = hit.collider.gameObject;
                Debug.Log("Hit " + letter.name + "!");

                IEnumerator drag = Drag(letter);
                stateMachine.StartCoroutine(drag);
            }
        }
        else
        {
            if(stateMachine.LetterHolding != null)
            {
                DragAndDropInteract(); 
            }

            Notification notification = new("ClickStopped", stateMachine.InputReader);
            NotificationCenter.Instance.PostNotification(notification);
        }
        */
    }

    protected IEnumerator CheckIfDrag()
    {
        yield return new WaitForSeconds(stateMachine.TimeToDragLetter);

        if(stateMachine.InputReader.MouseDown)
        {
            Drag(); 
        }
        else
        {
            Click(); 
        }
    }

    protected GameObject FindObjectWhereMouseIs()
    {
        /*
        GameObject objectHit = null;

        LayerMask layerMask = stateMachine.UICamera.cullingMask;
        Vector3 mousePos = stateMachine.InputReader.MousePosition;
        Ray ray = stateMachine.UICamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) // TODO: help performance with removing infinity
        {
            objectHit = hit.collider.gameObject;
            Debug.Log("Hit " + objectHit.name + "!");
        }
        else
        {
            LayerMask mainCamLayerMask = Camera.main.cullingMask;
            Ray mainCamRay = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(mainCamRay, out hit, Mathf.Infinity, mainCamLayerMask)) // TODO: limit how far away you can click something 
            {
                objectHit = hit.collider.gameObject;
                Debug.Log("Hit " + objectHit.name + "!");
            }
        }
        */ 

        GameObject objectHit = FindObjectWhereMouseIs(stateMachine.UICamera);

        if(objectHit == null)
        {
            objectHit = FindObjectWhereMouseIs(Camera.main); 
        }

        return objectHit; 
    }

    protected GameObject FindObjectWhereMouseIs(Camera camera)
    {
        GameObject objectHit = null;

        LayerMask layerMask = camera.cullingMask;
        Vector3 mousePos = stateMachine.InputReader.MousePosition;
        Ray ray = camera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) // TODO: help performance with removing infinity
        {
            objectHit = hit.collider.gameObject;
            Debug.Log("Hit " + objectHit.name + "!");
        }

        return objectHit;
    }

    protected void Click()
    {
        GameObject objectHit = FindObjectWhereMouseIs(); 

        if(objectHit != null && objectHit.TryGetComponent(out IClickable component))
        {
            component.Click(stateMachine);

            Notification notification = new("Clicked", component);
            notification.UserInfo["InputReader"] = stateMachine.InputReader;
            NotificationCenter.Instance.PostNotification(notification);
        }
        else
        {
            Debug.Log("object was not clickable"); 
        }
    }

    protected void Drag()
    {
        GameObject objectHit = FindObjectWhereMouseIs();

        if (objectHit != null && objectHit.TryGetComponent(out IDragable component))
        {
            component.Drag(stateMachine);

            Notification notification = new("Drag", component);
            notification.UserInfo["InputReader"] = stateMachine.InputReader;
            NotificationCenter.Instance.PostNotification(notification);
        }
        else
        {
            Debug.Log("object was not dragable");
            Click(); 
        }
    }

    protected void ClickRelease()
    {
        if(stateMachine.LetterHolding != null)
        {
            GameObject objectHit = FindObjectWhereMouseIs(Camera.main);

            if(objectHit != null && objectHit.TryGetComponent(out ITakesLetters component)) 
            {
                component.TakeLetter(stateMachine);
            }

            stateMachine.LetterHolding = null; 
        }

        Notification notification = new("ClickStopped", stateMachine.InputReader);
        NotificationCenter.Instance.PostNotification(notification);
    }

    protected void Dialog()
    {
        if(stateMachine.NPCInDialogWith != null && !stateMachine.NPCInDialogWith.Interact(stateMachine))
        {
            stateMachine.ExitDialog(); 
        }
    }

    protected void FlapWings()
    {
        if (_canFlap)
        {
            stateMachine.Animator.SetTrigger("Jumped");
            stateMachine.Velocity.y += stateMachine.FlapForce;

            _canFlap = false;

            IEnumerator validateFlapCoroutine = ValidateFlap();
            stateMachine.StartCoroutine(validateFlapCoroutine);
        }
    }

    protected void TakeOff()
    {
        stateMachine.Animator.SetTrigger("Jumped");
        stateMachine.Velocity.y += stateMachine.FlapForce * stateMachine.TakeOffMultiplier;
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
        stateMachine.InputReader.OnJumpPerformed += TakeOff;
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


// Dummy State for right now
public class StunnedState : PlayerBaseState
{
    Vector3 _direction;
    private float stunDuration = 5f;
    private float knockbackStrength = 10f;
    private float elapsedTime = 0f;
    private float gravity = -9.81f;

    public StunnedState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        _direction = new Vector3(0, 0, 0);
    }

    public StunnedState(PlayerStateMachine stateMachine, Vector3 direction) : base(stateMachine)
    {
        _direction = direction;
    }

    public override void Enter()
    {
        Debug.Log("Entered Stunned State");
        //IEnumerator coroutine = timer(); 
        //stateMachine.StartCoroutine(coroutine); 
    }

    public override void Update()
    {
        //ApplyGravity();

        if (elapsedTime < stunDuration)
        {
            Vector3 knockback = _direction * knockbackStrength;
            knockback.y += gravity * Time.deltaTime;
            stateMachine.Controller.Move(knockback * Time.deltaTime);

            elapsedTime += Time.deltaTime;
        }
        else
        {
            //switch state after stun ends
            if (stateMachine.Controller.isGrounded)
            {
                stateMachine.SwitchState(new PlayerGroundedState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerAirborneState(stateMachine));
            }
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited Stunned State");
    }

    /**public IEnumerator timer()
    {
        yield return new WaitForSeconds(5f);

        if(stateMachine.Controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerGroundedState(stateMachine)); 
        }
        else
        {
            stateMachine.SwitchState(new PlayerAirborneState(stateMachine));
        }
    }*/
}