using System;
using System.Collections.Generic;

public class Mailbox
{
	private List<Letter> _incomingMailbox;
    private Dictionary<string, List<Letter>> _outgoingMailbox;
	//public Dictionary<string, List<Letter>> OutgoingMailbox { get { return _outgoingMailbox; } }

    public Mailbox(Dictionary<string, List<Letter>> fromMailBox)
	{
		_incomingMailbox = new List<Letter>();
		_outgoingMailbox = fromMailBox; 
    }
	public Mailbox() : this(new Dictionary<string, List<Letter>>()) { }

	public void AddIncomingMail(Letter letter)
	{
		_incomingMailbox.Add(letter);
		ValidateLetters(letter); 
	}

    public void AddOutgoingMail(Letter letter)
    {
        if (_outgoingMailbox.ContainsKey(letter.To))
        {
            _outgoingMailbox[letter.To].Add(letter);
        }
        else
        {
            _outgoingMailbox[letter.To] = new List<Letter>
            {
                letter
            };
        }
    }

    private void ValidateLetters(Letter givenLetter)
	{
		foreach(KeyValuePair<string, List<Letter>> letters in _outgoingMailbox)
		{
			foreach(Letter letter in letters.Value)
			{
				letter.ValidatePrereqs(givenLetter); 
			}
		}
	}

	public List<Letter> GetOutgoingMail()
	{
		List<Letter> toReturn = new List<Letter>();

        foreach (KeyValuePair<string, List<Letter>> letters in _outgoingMailbox)
        {
            foreach (Letter letter in letters.Value)
            {
                if (letter.ValidatePrereqs())
                {
                    toReturn.Add(letter);
                }
            }
        }

        foreach (Letter letter in toReturn) RemoveFromOutGoingMail(letter); 
        return toReturn;
    }

    private void RemoveFromOutGoingMail(Letter letter)
    {
        _outgoingMailbox[letter.To].Remove(letter); 
    }

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
}

