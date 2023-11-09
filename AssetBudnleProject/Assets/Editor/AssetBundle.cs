using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetBundle : MonoBehaviour
{
	[MenuItem("Bundles/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{

		BuildPipeline.BuildAssetBundles("Assets/StreamingAssets", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
		AssetDatabase.Refresh();

		Debug.Log("AssetBundle Complete!");
	}
}
