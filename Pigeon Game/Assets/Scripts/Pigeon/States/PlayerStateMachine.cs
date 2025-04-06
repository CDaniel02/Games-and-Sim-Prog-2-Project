using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Cinemachine; 

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputReader))]
public class PlayerStateMachine : StateMachine
{
    public Vector3 Velocity;
    public float Drag = 0.35f; 
    public float GroundedMovementSpeed = 8f; 
    public float AirborneMovementSpeed = 30f;
    public float FlapForce = 15f;
    public float TakeOffMultiplier = 1.5f; 
    public float RotationalFactor = 5f; 
    public float LookRotationDampFactor { get; private set; } = 2;
    public Transform MainCamera { get; private set; }
    public InputReader InputReader { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public int interactRange = 5;
    public DialogBox dialogBox;
    public Camera UICamera;
    public float TimeToDragLetter = 0.1f;
    public NPC NPCInDialogWith; 

    public PlayerInput playerInput;

    private List<Letter> _letters;

    private Letter _letterHolding;

    public GameObject PausePanel;
    private bool paused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && paused == false)
        {
            PausePanel.SetActive(true);
            paused = true;
            Time.timeScale = 0f;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && paused == true)
        {
            PausePanel.SetActive(false);
            paused = false;
            Time.timeScale = 1f;
        }
    }


    public Letter LetterHolding
    {
        get
        {
            return _letterHolding;
        }
        set
        {
            _letterHolding = value;
        }
    }

    private bool _curserLocked;
    public bool CurserLocked
    {
        get
        {
            return _curserLocked; 
        }
        set
        {
            _curserLocked = value;

            if (_curserLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public CinemachineFreeLook CinemachineCamera; 
    

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>(); 
        MainCamera = Camera.main.transform;
        InputReader = GetComponent<InputReader>();
        Animator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();
        NPCInDialogWith = null;
        CurserLocked = true; 

        _letters = new List<Letter>(); 
        LetterHolding = null; 

        Cursor.lockState = CursorLockMode.Locked; 

        SwitchState(new PlayerAirborneState(this));
    }

    public void SwitchActionMap(string map)
    {
        playerInput.SwitchCurrentActionMap(map); 
    }

    public bool CheckAndGiveLetter(NPC npc, out Letter letter)
    {
        letter = new Letter(); 

        foreach(Letter currentLetter in _letters)
        {
            if(currentLetter.To == npc.Name)
            {
                letter = currentLetter; 
            }
        }

        return RemoveLetter(letter); 

        /*

        if(_letters.Remove(letter))
        {
            result = true;

            Notification notification = new("LetterRemoved", letter);
            NotificationCenter.Instance.PostNotification(notification);
        }

        */

        /*
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
        */ 

    }

    public bool RemoveLetter(Letter letter)
    {
        bool result = false;

        if (_letters.Remove(letter))
        {
            result = true;

            Notification notification = new("LetterRemoved", letter);
            NotificationCenter.Instance.PostNotification(notification);
        }

        return result;
    }

    public void AddLetter(Letter letter)
    {
        _letters.Add(letter); 

        Notification notification = new("LetterAdded", letter);
        NotificationCenter.Instance.PostNotification(notification);
    }

    public void EnterDialog(NPC npc)
    {
        NPCInDialogWith = npc;
        SwitchActionMap("Dialog"); 
    }

    public void ExitDialog()
    {
        NPCInDialogWith = null;
        SwitchActionMap("Player"); 
    }
}
