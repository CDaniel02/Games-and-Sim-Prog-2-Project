using System;

public interface IClickable
{
    public void Click(PlayerStateMachine stateMachine); 
}

public interface IDragable
{
    public void Drag(PlayerStateMachine stateMachine);
}

public interface ITakesLetters
{
    public void TakeLetter(PlayerStateMachine stateMachine);
}


