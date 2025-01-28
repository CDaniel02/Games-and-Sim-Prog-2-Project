using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    public NPCAction bestAction {  get;  set; }
    private NPCController npc;
    // Start is called before the first frame update
    void Start()
    {
        npc = GetComponent<NPCController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Loop through all the avalaible actions
    //Give me the highest scorest action
    public void DecideBestAction(NPCAction[] actionsAvaliable)
    {
        float score = 0f;
        int nextBestActionIndex = 0;
        for(int i = 0; i < actionsAvaliable.Length; i++)
        {
            if(ScoreAction(actionsAvaliable[i]) > score)
            {
                nextBestActionIndex = i;
                score = actionsAvaliable[i].Score;
            }
        }

        bestAction = actionsAvaliable[nextBestActionIndex];
    }

    //Loop through all the considerations of the action
    //Score all the considerations
    //Average the consideration scores ==> overall action score
    public float ScoreAction(NPCAction action)
    {
        float score = 1f;
        for(int i = 0; i < action.considerations.Length; i++)
        {
            float considerationScore = action.considerations[i].ScoreConsideration();
            score *= considerationScore;
        }
        if(score == 0f)
        {
            action.Score = 0f;
            return action.Score; //No point computing further
        }
        //Averaging scheme of overall score (invented by Dave Mark
        float originalScore = score;
        float modFactor = 1 - (1 / action.considerations.Length);
        float makeupValue = (1 - originalScore) * modFactor;
        action.Score = originalScore + (makeupValue * originalScore);

        return action.Score;
    }


}
