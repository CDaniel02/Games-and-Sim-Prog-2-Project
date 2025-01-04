using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputReader))]
public class PlayerStateMachine : StateMachine
{
    public Vector3 Velocity;
    [SerializeField]
    public float MovementSpeed { get; private set; } = 20f;
    [SerializeField]
    public float FlapForce { get; private set; } = 20f;
    [SerializeField]
    public float LookRotationDampFactor { get; private set; } = 10;
    public Transform MainCamera { get; private set; }
    public InputReader InputReader { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }

    private void Start()
    {

        MainCamera = Camera.main.transform;

        InputReader = GetComponent<InputReader>();


        Animator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();

        SwitchState(new PlayerAirborneState(this));
    }
}
