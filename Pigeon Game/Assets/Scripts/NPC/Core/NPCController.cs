using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public enum State
{
    idle,
    move
}

public class NPCController : MonoBehaviour
{
    public MoveController mover { get; set; }
    public AIBrain aiBrain { get; set; }
    public NPCAction[] actionsAvaliable;

    public State currentState { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<MoveController>();
        aiBrain = GetComponent<AIBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        FSMTick();
    }

    public void FSMTick()
    {
        if(currentState == State.idle)
        {

        }
        else if (currentState == State.move)
        {

        }
    }
}
