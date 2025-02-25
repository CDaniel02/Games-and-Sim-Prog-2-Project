using UnityEngine;
using System.Collections;
using TMPro; 

public class LetterScript : MonoBehaviour, IClickable, IDragable
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

	public void Click(PlayerStateMachine stateMachine)
	{
        Notification notification = new("LetterClicked", gameObject);
        notification.UserInfo["InputReader"] = stateMachine.InputReader;
        NotificationCenter.Instance.PostNotification(notification);
    }

    public void Drag(PlayerStateMachine stateMachine)
    {
        stateMachine.LetterHolding = thisLetter;
        Debug.Log("Holding letter " + stateMachine.LetterHolding.LetterId);

        Notification notification = new("LetterDrag", gameObject);
        notification.UserInfo["InputReader"] = stateMachine.InputReader;
        NotificationCenter.Instance.PostNotification(notification);
    }
}