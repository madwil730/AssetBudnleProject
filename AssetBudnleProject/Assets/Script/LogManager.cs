using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI statusMessage;
	[SerializeField]
	private Image block;
	[SerializeField]
	private GameObject yesButton;
	[SerializeField]
	private GameObject noButton;
	[SerializeField]
	private GameObject confirmButton;

	public void CallConfirmLog(string text,bool isbool)
	{
		CallMessageLog(text);
		yesButton.SetActive(!isbool);
		noButton.SetActive(!isbool);
		confirmButton.SetActive(isbool);
	}

	public void CallMessageLog(string text)
	{
		statusMessage.text = text;
	}

	public void CallSetBlock(bool isbool)
	{
		block.enabled = isbool;	
	}

	public void CallSetInterActive(bool isbool)
	{
		yesButton.GetComponent<Button>().interactable = isbool;
		noButton.GetComponent<Button>().interactable = isbool;
	}
}
