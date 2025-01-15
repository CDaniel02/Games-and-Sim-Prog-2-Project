using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Letter 
{
	public int LetterId; 

	private string _to; 
	public string To { get { return _to; } set { _to = value; } }

	private string _toResponse;
	public string ToResponse { get { return _toResponse; } set { _toResponse = value; } }

	private string _from; 
	public string From { get { return _from; } set { _from = value; } }

    private string _fromResponse;
    public string FromResponse { get { return _fromResponse; } set { _fromResponse = value; } }

    private string _body; 
	public string Body { get { return _body; } set { _body = value; } }

	private List<Letter> _prereqLetters;
	public List<Letter> PrereqLetters { get { return _prereqLetters; } set { _prereqLetters = value; } }

	// Designated Contructor 
	public Letter(string to, string from, string body)
	{
		To = to;
		From = from;
		Body = body;

		ToResponse = "";
		FromResponse = "";
		PrereqLetters = new List<Letter>(); 
	}
	public Letter(string to, string from) : this(to, from, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.") { }
	public Letter() : this("NO NAME", "NO NAME") { }

	public bool ValidatePrereqs(Letter letter)
	{
		bool result = false;

		Debug.Log(PrereqLetters.Count); 
		Debug.Log(PrereqLetters.Contains(letter));

		Debug.Log(PrereqLetters[0].To + ", " + PrereqLetters[0].From + ", " + PrereqLetters[0].ToResponse + ", " + PrereqLetters[0].FromResponse + ", " + PrereqLetters[0].Body);
        Debug.Log(letter.To + ", " + letter.From + ", " + letter.ToResponse + ", " + letter.FromResponse + ", " + letter.Body);

        foreach (Letter prereqLetter in PrereqLetters)
		{
			if(prereqLetter.Equals(letter))
			{
				Debug.Log(PrereqLetters.Remove(letter)); 
                Debug.Log("Letter removed");
				Debug.Log(PrereqLetters.Count); 
				Debug.Log(ValidatePrereqs()); 
            }
		}

		if(PrereqLetters.Count <= 0)
		{
			result = true; 
		}

		return result; 
	}

	public bool ValidatePrereqs()
	{
		return PrereqLetters.Count <= 0; 
	}

	public bool Equals(Letter letter)
	{
		return (To == letter.To) && (From == letter.From) && (Body == letter.Body) && (ToResponse == letter.ToResponse) && (FromResponse == letter.FromResponse); 
	}
}

