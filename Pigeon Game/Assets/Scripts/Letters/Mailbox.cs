using System.Collections.Generic;

// used for holding mail in NPCs
// outgoingmail may require certain incoming letters in order to be sent out

public class Mailbox
{
	private List<Letter> _incomingMailbox;
    private List<Letter> _outgoingMailbox;

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

	public void AddOutgoingMail(List<Letter> letters)
	{
		foreach(Letter letter in letters)
		{
			AddOutgoingMail(letter); 
		}
	}

	// checks all the letters in the outgoing mailbox to see if we've received a letter that is a prereq for another letter
    private void ValidateLetters(Letter givenLetter)
	{
		foreach(Letter letter in _outgoingMailbox)
		{
			letter.ValidatePrereqs(givenLetter);
		}
	}

    // retrieves all mail available to be sent out and deletes from the mailbox
	// essentially GIVES OUT avaiable mail
    public List<Letter> RemoveOutgoingMail()
	{
		List<Letter> toReturn = new List<Letter>();

        foreach (Letter letter in _outgoingMailbox)
        {
            if (letter.ValidatePrereqs())
            {
                toReturn.Add(letter);
            }
        }

        foreach (Letter letter in toReturn) _outgoingMailbox.Remove(letter);
        return toReturn;
    }
}

