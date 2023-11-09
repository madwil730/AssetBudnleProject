using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using TMPro;

public class PanelUIController : MonoBehaviour
{
	[SerializeField]
    private RectTransform model;
	[SerializeField]
	private RectTransform material;
	[SerializeField]
	private RectTransform download;
	[SerializeField]
	private float outXPostion;
	[SerializeField]
	private float inXPostion;
	[SerializeField]
	private float duration;


	[SerializeField]
	private LogManager logManager;

	private bool modelSelected;
	private bool costumeSelected;

	public void MoveModelPanel()
	{
		if(!modelSelected )
		{
			model.DOAnchorPosX(outXPostion, duration).SetEase(Ease.OutExpo);
			modelSelected = true;

			if(costumeSelected)
			{
				material.DOAnchorPosX(inXPostion, duration).SetEase(Ease.OutExpo);
				costumeSelected = false;
			}
		}
		else
		{
			model.DOAnchorPosX(inXPostion, duration).SetEase(Ease.OutExpo);
			modelSelected = false;
		}
	}

	public void MoveCostumePanel()
	{
		if (!costumeSelected)
		{
			material.DOAnchorPosX(outXPostion, duration).SetEase(Ease.OutExpo);
			costumeSelected = true;

			if (costumeSelected)
			{
				model.DOAnchorPosX(inXPostion, duration).SetEase(Ease.OutExpo);
				modelSelected = false;
			}
		}
		else
		{
			material.DOAnchorPosX(inXPostion, duration).SetEase(Ease.OutExpo);
			costumeSelected = false;
		}
	}

	public void ActivePanel(bool isbool)
	{
		if(isbool)
		{
			var str = "서버에서 에셋번들 데이터를 \r\n다운받으시겠습니까?";
			logManager.CallConfirmLog(str, false);
			logManager.CallSetBlock(true);
			logManager.CallSetInterActive(true);

			if (modelSelected)
				MoveModelPanel();

			if(costumeSelected)
				MoveCostumePanel();

		
			download.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
		}
		else
		{
			logManager.CallSetBlock(false);
			download.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutExpo);
		}
	}

	public void Exit()
	{
		Application.Quit();
	}
}
