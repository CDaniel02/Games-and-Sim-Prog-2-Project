
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputReader : MonoBehaviour 
{
    public Vector2 MouseDelta;
    public Vector2 MousePosition; 
    public Vector2 MoveComposite;

    public bool MouseDown; 

    public Action OnJumpPerformed;
    public Action OnClickPerformed; 
    public Action OnInteractPerformed;
    public Action OnUnlockCursorPerformed;
    public Action OnDialogPerformed;

    private void OnEnable()
    {
        MouseDelta = new UnityEngine.Vector2(0, 0);
        MoveComposite = new UnityEngine.Vector2(0, 0);
        MousePosition = new Vector2(0, 0);
        MouseDown = false; 
        OnJumpPerformedClear();
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

    public void OnDialog(InputValue inputValue)
    {
        if (!inputValue.isPressed)
            return;

        OnDialogPerformed?.Invoke();
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
        OnClickPerformed?.Invoke();
    }

    private void Nothing()
    {

    }
}
