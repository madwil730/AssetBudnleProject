using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class CreateThumbnail : MonoBehaviour
{
	public MeshRenderer mesh;
	public Material mat;
	public RenderTexture DrawTexture;


	
	public void CreateThumbnails()
	{
		
		StartCoroutine(CoCreateThumbnail());
	}

	private IEnumerator CoCreateThumbnail()
	{
		if(mat != null)
		{
			mesh.material = mat;
			yield return new WaitForEndOfFrame();
			var path = Application.dataPath + $"/Thumbnail/{mesh.name}_{mat.name}.png";
			RenderTexture.active = DrawTexture;
			var texture2D = new Texture2D(DrawTexture.width, DrawTexture.height);
			texture2D.ReadPixels(new Rect(0, 0, DrawTexture.width, DrawTexture.height), 0, 0);
			texture2D.Apply();
			var data = texture2D.EncodeToPNG();
			File.WriteAllBytes(path, data);
		}
		else
		{
			yield return new WaitForEndOfFrame();
			var path = Application.dataPath + $"/Thumbnail/{mesh.name}.png";
			RenderTexture.active = DrawTexture;
			var texture2D = new Texture2D(DrawTexture.width, DrawTexture.height);
			texture2D.ReadPixels(new Rect(0, 0, DrawTexture.width, DrawTexture.height), 0, 0);
			texture2D.Apply();
			var data = texture2D.EncodeToPNG();
			File.WriteAllBytes(path, data);
		}

		Debug.Log("Thumbnail Created!");
	}
}

