using Firebase.Storage;
using System;
using System.Collections;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;

public class FireBaseDownload : MonoBehaviour
{
	[SerializeField]
	private AssetBundleLoader load;
	[SerializeField]
	private LogManager logManager;

	private AssetBundle assetBundle;


	public void AssetDownLoad()
	{
		StartCoroutine(download());
	}

	private IEnumerator download()
	{
		logManager.CallMessageLog("다운로드 중..");
		logManager.CallSetInterActive(false);

		var path = Path.Combine(Directory.GetCurrentDirectory(), Application.streamingAssetsPath);


		// manifast
		if(assetBundle != null)
			assetBundle.Unload(true);

		assetBundle = AssetBundle.LoadFromFile(path + "/Manifast");
		var manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		var modelhash = manifest.GetAssetBundleHash("model").ToString();
		var materialhash = manifest.GetAssetBundleHash("material").ToString();
		var texturehash = manifest.GetAssetBundleHash("png").ToString();

		FirebaseStorage storage = FirebaseStorage.DefaultInstance;
		var storageReference = storage.GetReference("manifast");
		var downloadTask = storageReference.GetFileAsync(path + "/Manifast",
		new StorageProgress<DownloadState>(state =>
		{
			Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
		}), CancellationToken.None);

		yield return new WaitUntil(() => downloadTask.IsCompleted);

		if (assetBundle != null)
			assetBundle.Unload(true);

		assetBundle = AssetBundle.LoadFromFile(path + "/Manifast");
		var newmanifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		var modelhash_new = newmanifest.GetAssetBundleHash("model").ToString();
		var materialhash_new = newmanifest.GetAssetBundleHash("material").ToString();
		var texturehash_new = newmanifest.GetAssetBundleHash("png").ToString();


		if (modelhash == modelhash_new && materialhash == materialhash_new && texturehash == texturehash_new)
		{
			var str = "현재 최신 에셋번들 버전이 \n적용되고 있습니다";
			logManager.CallConfirmLog(str, true);

			yield break;
		}

		Debug.Log("##########manifast downloaded#############");

		// model
		if (modelhash != modelhash_new)
		{
			File.Delete(path + "/model");
			if (!File.Exists(path + "/model"))
				Debug.Log("model file is delete");

			storageReference = storage.GetReference("model");

			downloadTask = storageReference.GetFileAsync(path + "/model",
			new StorageProgress<DownloadState>(state =>
			{
				Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
			}), CancellationToken.None);

			yield return new WaitUntil(() => downloadTask.IsCompleted);
			Debug.Log("##########model downloaded#############");
		}

		// material
		if (materialhash != materialhash_new)
		{
			File.Delete(path + "/material");
			if (!File.Exists(path + "/material"))
				Debug.Log("material file is delete");

			storageReference = storage.GetReference("material");
			downloadTask = storageReference.GetFileAsync(path + "/material",
			new StorageProgress<DownloadState>(state =>
			{
				Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
			}), CancellationToken.None);

			yield return new WaitUntil(() => downloadTask.IsCompleted);
			Debug.Log("##########material downloaded#############");
		}


		//texture

		if (texturehash != texturehash_new)
		{
			File.Delete(path + "/png");
			if (!File.Exists(path + "/png"))
				Debug.Log("png file is delete");


			storageReference = storage.GetReference("png");
			downloadTask = storageReference.GetFileAsync(path + "/png",
			new StorageProgress<DownloadState>(state =>
			{
				Debug.Log(String.Format("Progress: {0} of {1} bytes transferred.", state.BytesTransferred, state.TotalByteCount));
			}), CancellationToken.None);

			yield return new WaitUntil(() => downloadTask.IsCompleted);
			Debug.Log("##########texture downloaded#############");
		}
			
		logManager.CallConfirmLog("다운로드 완료", true);
		load.AssetBundleLoad();
	}
}
