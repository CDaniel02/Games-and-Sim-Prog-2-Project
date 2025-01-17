using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public MoveController mover { get; set; }
    public AIBrain aiBrain { get; set; }
    public NPCAction[] actionsAvaliable;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<MoveController>();
        aiBrain = GetComponent<AIBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
