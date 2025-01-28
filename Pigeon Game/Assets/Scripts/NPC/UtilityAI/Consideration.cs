using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consideration : ScriptableObject
{
    public string Name;

    private float _score;
    public float Score
    {
        get { return _score; }
        set
        {
            this._score = Mathf.Clamp01(value);
        }
    }
    public virtual void Awake()
    {
        Score = 0;
    }

    public abstract float ScoreConsideration();
}
