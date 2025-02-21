using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LetterDisplay : MonoBehaviour
{
	public GameObject LetterPrefab;

	public float LetterSpeed = 10f; 
	public int FirstLetterX = -550;
	public int LetterY = -235;
	public int LetterZ = 50;
	public int DisplayLetterX = -255; 
	public int DisplayLetterY = 160;
	public int DisplayLetterZ = -225;

	// TODO: make this dynamic to screen size
	// but make all the letters center most of the time
	public int LetterDistanceApart = 220;
	public int MaxLettersBeforeOverlap = 6;
	public int TotalLetterDistance; 

    private List<GameObject> _letters;
	private GameObject _displayLetter; 
    private GameObject LetterOnDisplay
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

		NotificationCenter.Instance.AddObserver("LetterAdded", LetterAdded);
        NotificationCenter.Instance.AddObserver("LetterRemoved", LetterRemoved);
        NotificationCenter.Instance.AddObserver("LetterClicked", LetterClicked);

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

        if (LetterOnDisplay != null)
        {
            Vector3 displayLetterPos = new Vector3(DisplayLetterX, DisplayLetterY, DisplayLetterZ);
            _displayLetter.transform.localPosition = Vector3.Lerp( _displayLetter.transform.localPosition, displayLetterPos, LetterSpeed * Time.deltaTime);
            //_displayLetter.transform.localPosition = new Vector3(FirstLetterX, DisplayLetterY, DisplayLetterZ);
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
			newLetter.GetComponent<LetterTextFiller>().SetLetter(letter); 
            _letters.Add(newLetter);
        }
	}

    public void LetterRemoved(Notification notification)
    {
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
				letterToRemove = LetterOnDisplay; 
                LetterOnDisplay = null;
            }
            _letters.Remove(letterToRemove);
            Destroy(letterToRemove);
        }
    }

	public void LetterClicked(Notification notification)
	{
		GameObject letter = (GameObject)notification.Object;
		InputReader inputReader = (InputReader)notification.UserInfo["InputReader"];
		if (letter != null)
		{
			if(LetterOnDisplay != null && LetterOnDisplay.Equals(letter))
			{
				LetterOnDisplay = null; 
			}
			else
			{
				LetterOnDisplay = letter;
			}
        }
    }
}

