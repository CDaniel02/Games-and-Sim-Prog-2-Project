using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Letter 
{
	private string _to; 
	public string To { get { return _to; } set { _to = value; } }

	private string _toResponse;
	public string ToReponse { get { return _toResponse; } set { _toResponse = value; } }

	private string _from; 
	public string From { get { return _from; } set { _from = value; } }

    private string _fromResponse;
    public string FromReponse { get { return _fromResponse; } set { _fromResponse = value; } }

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

		ToReponse = "";
		FromReponse = "";
		PrereqLetters = new List<Letter>(); 
	}
	public Letter(string to, string from) : this(to, from, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.") { }
	public Letter() : this("NO NAME", "NO NAME") { }

	public bool ValidatePrereqs(List<Letter> list)
	{
		bool result = false; 

		foreach( Letter letter in list)
		{
			if(PrereqLetters.Contains(letter))
			{
				PrereqLetters.Remove(letter); 
			}
		}

		if(PrereqLetters.Count <= 0)
		{
			result = true; 
		}

		return result; 
	}


}

