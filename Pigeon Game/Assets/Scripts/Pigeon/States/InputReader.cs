
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls; 

public class InputReader : MonoBehaviour // , Controls.IPlayerActions
{
    public Vector2 MouseDelta;
    public Vector2 MousePosition; 
    public Vector3 LastClickedPoint; 
    public Vector2 MoveComposite;

    public bool MouseDown; 

    public Action OnJumpPerformed;
    public Action OnClickPerformed; 
    public Action OnInteractPerformed;
    public Action OnUnlockCursorPerformed; 

    //private Controls controls;

    private void OnEnable()
    {
        MouseDelta = new UnityEngine.Vector2(0, 0);
        MoveComposite = new UnityEngine.Vector2(0, 0);
        MousePosition = new Vector2(0, 0);
        LastClickedPoint = new Vector3(0, 0, 0);
        MouseDown = false; 
        OnJumpPerformedClear();
        //MouseDelta
    //    if (controls != null)
    //        return;

    //    controls = new Controls();
    //    controls.Player.SetCallbacks(this);
    //    controls.Player.Enable();
    }

    public void OnDisable()
    {
    //    controls.Player.Disable();
    }

    public void OnLook(InputValue inputValue)
    {
        MouseDelta = inputValue.Get<UnityEngine.Vector2>();
    }

    public void OnMove(InputValue inputValue)
    {
        MoveComposite = inputValue.Get<UnityEngine.Vector2>();
    }

    public void OnMouse(InputValue inputValue)
    {
        MousePosition = inputValue.Get<Vector2>();
    }

    public void OnInteract(InputValue inputValue)
    {
        if (!inputValue.isPressed)
            return;

        OnInteractPerformed?.Invoke();
    }

    public void OnUnlockCursor(InputValue inputValue)
    {
        if (!inputValue.isPressed)
            return;

        OnUnlockCursorPerformed?.Invoke();
    }

    public void OnJump(InputValue inputValue)
    {
        if (!inputValue.isPressed)
            return;

        OnJumpPerformed?.Invoke();
    }

    public void OnJumpPerformedClear()
    {
        OnJumpPerformed = Nothing; 
    }

    public void OnClick(InputValue inputValue)
    {
        MouseDown = inputValue.isPressed; 

        if (!inputValue.isPressed)
            return;

        LastClickedPoint = MousePosition; 
        OnClickPerformed?.Invoke(); 

        //Vector3 mousePos = Mouse.current.position.ReadValue();
        //mousePos.z = Camera.main.nearClipPlane;
            // Debug.Log(mousePos);
            //Debug.Log(mousePos);
            //Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            //Debug.Log(Worldpos); 
        
    }

    private void Nothing()
    {

    }
}
