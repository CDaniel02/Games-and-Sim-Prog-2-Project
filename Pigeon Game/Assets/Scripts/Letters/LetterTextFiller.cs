using UnityEngine;
using System.Collections;
using TMPro; 

public class LetterTextFiller : MonoBehaviour
{
	public TMP_Text ToTextMesh;
    public TMP_Text FromTextMesh;
    public TMP_Text BodyTextMesh;

	public Letter thisLetter; 

	public void SetLetter(Letter letter)
	{
		thisLetter = letter;
		gameObject.name = thisLetter.LetterId + ""; 
		ToTextMesh.text = "To: " + thisLetter.To;
		FromTextMesh.text = "From: " + thisLetter.From;
		BodyTextMesh.text = thisLetter.Body; 
	}
}

