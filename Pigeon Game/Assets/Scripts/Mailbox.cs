using System;
using System.Collections.Generic;

public class Mailbox
{
	private List<Letter> _incomingMailbox;
    private List<Letter> _outgoingMailbox;
	//public Dictionary<string, List<Letter>> OutgoingMailbox { get { return _outgoingMailbox; } }

    public Mailbox(List<Letter> fromMailBox)
	{
		_incomingMailbox = new List<Letter>();
		_outgoingMailbox = fromMailBox; 
    }
	public Mailbox() : this(new List<Letter>()) { }

	public void AddIncomingMail(Letter letter)
	{
		_incomingMailbox.Add(letter);
		ValidateLetters(letter); 
	}

    public void AddOutgoingMail(Letter letter)
    {
        _outgoingMailbox.Add(letter);
    }

    private void ValidateLetters(Letter givenLetter)
	{
		//foreach(KeyValuePair<string, List<Letter>> letters in _outgoingMailbox)
		//{
		foreach(Letter letter in _outgoingMailbox)
		{
			letter.ValidatePrereqs(givenLetter); 
		}
		//}
	}

	public List<Letter> GetOutgoingMail()
	{
		List<Letter> toReturn = new List<Letter>();

        //foreach (KeyValuePair<string, List<Letter>> letters in _outgoingMailbox)
        //{
        foreach (Letter letter in _outgoingMailbox)
        {
            if (letter.ValidatePrereqs())
            {
                toReturn.Add(letter);
            }
        }
        //}

        foreach (Letter letter in toReturn) _outgoingMailbox.Remove(letter);
        return toReturn;
    }

    //private void RemoveFromOutGoingMail(Letter letter)
    //{
    //    _outgoingMailbox.Remove(letter); 
    //}

    /*
	public int CountOfOutgoingMail()
	{
        int count = 0;

        foreach (KeyValuePair<string, List<Letter>> letters in _outgoingMailbox)
        {
            foreach (Letter letter in letters.Value)
            {
                if (letter.ValidatePrereqs())
                {
                    count++; 
                }
            }
        }

        return count;
    }
    */ 
}

