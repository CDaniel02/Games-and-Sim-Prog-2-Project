using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputReader))]
public class PlayerStateMachine : StateMachine
{
    public Vector3 Velocity;
    public float Drag = 0.35f; 
    public float GroundedMovementSpeed = 8f; 
    public float AirborneMovementSpeed = 20f;
    public float FlapForce = 15f;
    public float RotationalFactor = 5f; 
    public float LookRotationDampFactor { get; private set; } = 2;
    public Transform MainCamera { get; private set; }
    public InputReader InputReader { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public int interactRange = 5;
    public DialogBox dialogBox;

    public PlayerInput playerInput;

    private Dictionary<string, Letter> _letters;
    public Dictionary<string, Letter> Letters { get { return _letters; } set { _letters = value; } }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>(); 
        MainCamera = Camera.main.transform;
        InputReader = GetComponent<InputReader>();
        Animator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();

        Letters = new Dictionary<string, Letter>(); 

        SwitchState(new PlayerAirborneState(this));
    }

    public void SwitchActionMap(string map)
    {
        playerInput.SwitchCurrentActionMap(map); 
    }

    public bool CheckAndGiveLetter(NPC npc, out Letter letter)
    {
        bool result = false; 

        if(_letters.ContainsKey(npc.Name))
        {
            letter = _letters[npc.Name];
            _letters.Remove(npc.Name); 
            result = true; 
        }
        else
        {
            letter = new Letter(); 
        }

        return result; 
    }
}
