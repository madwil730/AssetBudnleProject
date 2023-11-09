using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour, IScrollHandler, IDragHandler
{
	[SerializeField]
	private Camera Camera;
	[Header("Camera Zoom")]
	[SerializeField]
	private float zoomSpeed = 10.0f;
	[SerializeField]
	private float zoomInlimit = 10f;
	[SerializeField]
	private float zoomOutlimit = 90f;
	[Header("Camera Init")]
	[SerializeField]
	private Vector3 InitPostion;
	[SerializeField]
	private float fieldOfView;


	//-----------------------------

	[SerializeField] private float speed = 30f;
	private readonly float thresholdY = 0.9f;
	private readonly float clampY = 90f;

	[Header("Environment")]
	[SerializeField]
	private Transform pivot;
	private Transform model;
	private float maxMouseXSpeed = 15f;
	private float maxMouseYSpeed = 8f;
	[SerializeField]
	private float smoothTime = 8f;

	private Vector3 currentEulerAngle = Vector3.zero;
	private Vector3 cameraEulerAngle = Vector3.zero;

	public Vector3 AvatarEulerAngle
	{
		get => currentEulerAngle;
		set { currentEulerAngle = value; }
	}

	public Vector3 CameraEulerAngle
	{
		get => cameraEulerAngle;
		set { cameraEulerAngle = value; }
	}


	public void OnModelLoad(Transform transform)
	{
		model = transform;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if ( pivot != null && Input.touchCount < 2)
		{
			float x = speed * eventData.delta.x * Time.fixedDeltaTime;
			float y = speed * eventData.delta.y * Time.fixedDeltaTime;

			if (Mathf.Abs(x) > 0 && Mathf.Abs(y) < thresholdY)
				y = 0;

			currentEulerAngle.y -= Mathf.Clamp(x, -maxMouseXSpeed, maxMouseXSpeed);
			cameraEulerAngle.x += Mathf.Clamp(y, -maxMouseYSpeed, maxMouseYSpeed);
			cameraEulerAngle.x = Mathf.Clamp(cameraEulerAngle.x, -clampY, clampY);
			currentEulerAngle.x = 0f;

			Rotate();
		}
	}

	private void Rotate()
	{
		if (model == null)
			return;
		Quaternion resultRot = Quaternion.Euler(0, currentEulerAngle.y, 0);
		Quaternion cameraRot = Quaternion.Euler(-cameraEulerAngle.x, 0, 0);
		model.rotation = Quaternion.Lerp(model.rotation, resultRot, smoothTime * Time.fixedDeltaTime);
		pivot.rotation = Quaternion.Lerp(pivot.rotation, cameraRot, smoothTime * Time.fixedDeltaTime);
	}

	public void OnScroll(PointerEventData eventData)
	{
		float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

		// ÁÜ ÀÎ 
		if (Camera.fieldOfView <= zoomInlimit && scroll > 0)
		{
			Camera.fieldOfView = zoomInlimit;
		}
		//  ÁÜ¾Æ¿ô
		else if (Camera.fieldOfView >= zoomOutlimit && scroll < 0)
		{
			Camera.fieldOfView = zoomOutlimit;
		}
		else
			Camera.fieldOfView -= scroll;
	}

}
