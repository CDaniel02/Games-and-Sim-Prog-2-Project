using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Reflection;

public class Letter 
{
	private int _letterId; 
	public int LetterId { get { return _letterId; } set { _letterId = value; } }

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

	private List<int> _prereqLetters;
	public List<int> PrereqLetters
	{
		get
		{
			return _prereqLetters;
		}
		set
		{
			_prereqLetters = value; 
		}
	}

	// Designated Contructor 
	public Letter(string to, string from, string body)
	{
		To = to;
		From = from;
		Body = body;

		ToResponse = "";
		FromResponse = "";
		PrereqLetters = new List<int>();


		LetterId = -1; 
	}
	public Letter(string to, string from) : this(to, from, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.") { }
	public Letter() : this("NO NAME", "NO NAME") { }

	public bool ValidatePrereqs(Letter letter)
	{
        PrereqLetters.Remove(letter.LetterId);

        return ValidatePrereqs(); 
	}

	public bool ValidatePrereqs()
	{
		return PrereqLetters.Count <= 0; 
	}

	// get a property with a string
	// used like letterObject["From"] = "The King"
    public object this[string propertyName]
    {
        get
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            return property.GetValue(this, null);
        }
        set
        {
            PropertyInfo property = GetType().GetProperty(propertyName);
            Type propType = property.PropertyType;
            if (value == null)
            {
                if (propType.IsValueType && Nullable.GetUnderlyingType(propType) == null)
                {
                    throw new InvalidCastException();
                }
                else
                {
                    property.SetValue(this, null, null);
                }
            }
            else if (value.GetType() == propType)
            {
                property.SetValue(this, value, null);
            }
            else
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(propType);
                object propValue = typeConverter.ConvertFromString(value.ToString());
                property.SetValue(this, propValue, null);
            }
        }
    }
	//

    public bool Equals(Letter letter)
	{
		return (To == letter.To) && (From == letter.From) && (Body == letter.Body) && (ToResponse == letter.ToResponse) && (FromResponse == letter.FromResponse); 
	}
}

