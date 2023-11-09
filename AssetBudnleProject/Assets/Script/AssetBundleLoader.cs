using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class AssetBundleLoader : MonoBehaviour
{
	[SerializeField]
	private string AssetBundle_Prefab;
	[SerializeField]
	private string AssetBundle_Material;
	[SerializeField]
	private string AssetBundle_Texture;
	[SerializeField]
	private string AssetBundle_Manifast;
	[SerializeField]
	private modelViewerList settingMaterial;
	[SerializeField]
	private CameraController cameraController;


	private AssetBundle prefabBundle;
	private AssetBundle materialBundle;
	private AssetBundle textureBundle;

	void Start()
	{
		AssetBundleLoad();
	}

	public void AssetBundleLoad()
	{
		var path = Path.Combine(Directory.GetCurrentDirectory(), Application.streamingAssetsPath);
		string prefabPath = Path.Combine(path, AssetBundle_Prefab);
		string materialPath = Path.Combine(path, AssetBundle_Material);
		string texturePath = Path.Combine(path, AssetBundle_Texture);

		StartCoroutine(LoadFromMemoryAsync(prefabPath, materialPath, texturePath));
	}

	private IEnumerator LoadFromMemoryAsync(string prefabPath,string materialPath,string texturePath)
	{
		//model
		UnityWebRequest prefabRequest = UnityWebRequestAssetBundle.GetAssetBundle(prefabPath);
		yield return prefabRequest.SendWebRequest();

		if (prefabBundle != null)
			prefabBundle.Unload(true);

		prefabBundle = DownloadHandlerAssetBundle.GetContent(prefabRequest);
		GameObject[] prefab = prefabBundle.LoadAllAssets<GameObject>();

		//material
		UnityWebRequest materialRequest = UnityWebRequestAssetBundle.GetAssetBundle(materialPath);
		yield return materialRequest.SendWebRequest();

		if (materialBundle != null)
			materialBundle.Unload(true);

		materialBundle = DownloadHandlerAssetBundle.GetContent(materialRequest);
		Material[] mat = materialBundle.LoadAllAssets<Material>();

		//texture
		UnityWebRequest textureRequest = UnityWebRequestAssetBundle.GetAssetBundle(texturePath);
		yield return textureRequest.SendWebRequest();

		if (textureBundle != null)
			textureBundle.Unload(true);

		textureBundle = DownloadHandlerAssetBundle.GetContent(textureRequest);
		Texture2D[] tex = textureBundle.LoadAllAssets<Texture2D>();

		settingMaterial.OnModelLoad(mat, tex, prefab);

	}
}
