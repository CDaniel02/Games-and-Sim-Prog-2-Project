
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls; 

public class InputReader : MonoBehaviour // , Controls.IPlayerActions
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;

    public Action OnJumpPerformed;

    //private Controls controls;

    private void OnEnable()
    {
        MouseDelta = new UnityEngine.Vector2(0, 0);
        MoveComposite = new UnityEngine.Vector2(0, 0);
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

    private void Nothing()
    {

    }
}
