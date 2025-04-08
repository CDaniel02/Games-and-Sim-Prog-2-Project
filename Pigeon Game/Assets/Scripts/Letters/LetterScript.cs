using UnityEngine;
using System.Collections;
using TMPro; 

public class LetterScript : MonoBehaviour, IClickable, IDragable
{
	public TMP_Text ToTextMesh;
    public TMP_Text FromTextMesh;
    public TMP_Text BodyTextMesh;

    public RectTransform LetterText;
    public GameObject LetterPage; 

	public Letter thisLetter;

    public int CharacterLimitBeforeExpanding = 220;
    public int CharactersPerLine = 23;
    public float TextHeightIncrease = 20f;
    public float PageScaleIncrease = 0.10f;

	public void SetLetter(Letter letter)
	{
		thisLetter = letter;
		gameObject.name = thisLetter.LetterId + ""; 
		ToTextMesh.text = "To: " + thisLetter.To;
		FromTextMesh.text = "From: " + thisLetter.From;
		BodyTextMesh.text = thisLetter.Body;
        AdjustSize(); 
	}

    public void AdjustSize()
    {
        if(thisLetter.Body.Length > CharacterLimitBeforeExpanding)
        {
            int lines = (int)Mathf.Ceil((thisLetter.Body.Length - CharacterLimitBeforeExpanding) / CharactersPerLine);

            //LetterText.rect.height = LetterText.rect.height + lines * TextHeightIncrease;
            //LetterPage.transform.localScale.y = LetterPage.transform.localScale.y + lines * PageScaleIncrease;
            
        }
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