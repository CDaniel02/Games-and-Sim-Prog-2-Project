using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move: NPCAction
{
    public override void Execute(NPCController npc)
    {
        npc.Move();
    }
}
