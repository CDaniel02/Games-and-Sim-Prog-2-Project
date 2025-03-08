using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LetterDisplay : MonoBehaviour
{
	public float testZ = 685; 

	public GameObject LetterPrefab;

	public Camera UICamera; 

	public float LetterSpeed = 10f; 
	public int FirstLetterX = -550;
	public int LetterY = -235;
	public int LetterZ = 50;
	public int DisplayLetterX = -255; 
	public int DisplayLetterY = 160;
	public int DisplayLetterZ = -225;
	public int HoldingLetterZ = 0; 

	// TODO: make this dynamic to screen size
	// but make all the letters center most of the time
	public int LetterDistanceApart = 220;
	public int MaxLettersBeforeOverlap = 6;
	public int TotalLetterDistance;

	private InputReader _inputReader;

    private List<GameObject> _letters;

	private GameObject _letterHolding;
    private GameObject LetterHolding
    {
        set
        {
            if (_letterHolding != null)
            {
                _letters.Add(_letterHolding);
            }
            _letterHolding = value;
            _letters.Remove(_letterHolding);
        }

        get
        {
            return _letterHolding;
        }
    }

    private GameObject _displayLetter; 
    private GameObject DisplayLetter
	{
		set
		{
			if(_displayLetter != null)
			{
                _letters.Add(_displayLetter);
            }
            _displayLetter = value;
            _letters.Remove(_displayLetter);
		}

		get
		{
			return _displayLetter; 
		}
	}

	void Start()
	{
		_letters = new List<GameObject>();
		_displayLetter = null;
		_letterHolding = null; 
		_inputReader = null; 


        NotificationCenter.Instance.AddObserver("LetterAdded", LetterAdded);
        NotificationCenter.Instance.AddObserver("LetterRemoved", LetterRemoved);
        NotificationCenter.Instance.AddObserver("LetterClicked", LetterClicked);
        NotificationCenter.Instance.AddObserver("LetterDrag", LetterDrag);
        NotificationCenter.Instance.AddObserver("ClickStopped", ClickStopped);

        TotalLetterDistance = LetterDistanceApart * MaxLettersBeforeOverlap;
    }

	void Update()
	{
		// bobble letters?
		// continue to keep them in view

		// letters stay in their spot
		// letter on display goes to a special little spot

		if (_letters.Count > 0)
		{
			int letterXValue = FirstLetterX;
			int letterDistance = 0; 
			if(_letters.Count > MaxLettersBeforeOverlap)
			{
                letterDistance = TotalLetterDistance / _letters.Count;
            }
			else
			{
                letterDistance = LetterDistanceApart;
            }
            foreach (GameObject letter in _letters)
			{
				letter.transform.localPosition = Vector3.Lerp(letter.transform.localPosition, new Vector3(letterXValue, LetterY, LetterZ), LetterSpeed * Time.deltaTime);
				letterXValue += letterDistance;
			}
		}

        if (DisplayLetter != null)
        {
            Vector3 displayLetterPos = new Vector3(DisplayLetterX, DisplayLetterY, DisplayLetterZ);
            DisplayLetter.transform.localPosition = Vector3.Lerp( DisplayLetter.transform.localPosition, displayLetterPos, LetterSpeed * Time.deltaTime);
            //_displayLetter.transform.localPosition = new Vector3(FirstLetterX, DisplayLetterY, DisplayLetterZ);
        }

		if (LetterHolding != null)
		{
			if(_inputReader != null)
			{
				// track the letter to the mouse
				// TODO: offset letter so the mouse is in the middle of the letter 

				Vector3 mousePos = UICamera.ScreenToWorldPoint(new(_inputReader.MousePosition.x, _inputReader.MousePosition.y, UICamera.scaledPixelHeight));
				mousePos -= LetterHolding.transform.position; 
				mousePos.z = HoldingLetterZ;
				LetterHolding.transform.localPosition = Vector3.Lerp(LetterHolding.transform.localPosition, mousePos, LetterSpeed * Time.deltaTime);
            }
			else
			{
				Debug.Log("UI has returned holding letter"); 
				// end of dragging the letter
				LetterHolding = null; 
			}
		}
    }

	public void LetterAdded(Notification notification)
	{
		Letter letter = (Letter)notification.Object;
		if(letter != null)
		{
			// make a new gameobject and add that letter into the list and the scene
			Debug.Log("You received letter " + letter.LetterId);

            GameObject newLetter = Instantiate(LetterPrefab, gameObject.transform);
			newLetter.GetComponent<LetterScript>().SetLetter(letter); 
            _letters.Add(newLetter);
        }
	}

    public void LetterRemoved(Notification notification)
    {
		/*
        Letter letter = (Letter)notification.Object;
        if (letter != null)
        {
			// remove exisiting letter from list and from the scene 
			Debug.Log("You gave away letter " + letter.LetterId);

			GameObject letterToRemove = null;
			foreach(GameObject currentLetter in _letters)
			{
				if(currentLetter.name == letter.LetterId + "")
				{
					letterToRemove = currentLetter; 
				}
			}

			if(letterToRemove == null)
			{
				if(LetterOnDisplay.name == letter.LetterId + "")
				{
                    letterToRemove = LetterOnDisplay;
                    LetterOnDisplay = null;
                }
				else
				{
					letterToRemove = LetterHolding;
					LetterHolding = null; 
				}
				
            }

            _letters.Remove(letterToRemove);
            Destroy(letterToRemove);
        }
		*/

		GameObject letterToDestroy = LetterHolding; 
        LetterHolding = null;
        _letters.Remove(letterToDestroy);
        Destroy(letterToDestroy);
    }

	public void LetterClicked(Notification notification)
	{
		GameObject letter = (GameObject)notification.Object;
		// InputReader inputReader = (InputReader)notification.UserInfo["InputReader"];
		if (letter != null)
		{
			if(DisplayLetter != null && DisplayLetter.Equals(letter))
			{
				DisplayLetter = null; 
			}
			else
			{
				DisplayLetter = letter;
			}
        }
    }

	public void LetterDrag(Notification notification)
	{
        GameObject letter = (GameObject)notification.Object;
        InputReader inputReader = (InputReader)notification.UserInfo["InputReader"];
        if (letter != null)
        {
			Debug.Log("UI is holding a letter");
			if (DisplayLetter != null && DisplayLetter.Equals(letter))
			{
				DisplayLetter = null; 
			}
			_inputReader = inputReader;
			LetterHolding = letter; 
        }
    }

	public void ClickStopped(Notification notification)
	{
		_inputReader = null; 
	}
}

