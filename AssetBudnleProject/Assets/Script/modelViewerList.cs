using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class modelViewerList : MonoBehaviour
{
	private MeshRenderer modelMesh;
	private Material[] materialList;
	private Texture2D[] textureList;
	private GameObject instance;
	private string matName;

	public Transform CostumePanel;
	public Transform ModelPanel;
	public GameObject ButtonPrefab;
	public CameraController CameraController;

	public void OnModelLoad(Material[] mat, Texture2D[] tex, GameObject[] model)
	{
		if (materialList != null)
			Array.Clear(materialList, 0, materialList.Length);

		else if (textureList != null)
			Array.Clear(textureList, 0, textureList.Length);

		materialList = mat;
		textureList = tex;
		CreateModelUI(model);
	}
	private void CreateModelUI(GameObject[] model)
	{
		if(ModelPanel.childCount>0)
		{
			for (int i = 0; i < ModelPanel.childCount; i++)
			{
				Destroy(ModelPanel.GetChild(i).gameObject);
			}
		}

		foreach (var (value, index) in model.Select((value, index) => (value, index)))
		{
			var modelData = model[index].GetComponent<modelData>();
			var modelUI = Instantiate(ButtonPrefab, ModelPanel);

			var button = modelUI.GetComponentInChildren<Button>();
			button.GetComponentInChildren<TextMeshProUGUI>().text = modelData.ModelName;
			button.onClick.AddListener(() =>
			{
				if (model[index].name == modelData.ModelName)
				{
					Destory();
					instance = Instantiate(model[index], modelData.SpawnPosition, Quaternion.identity);
					modelMesh = instance.GetComponent<MeshRenderer>();
					CameraController.OnModelLoad(instance.transform);
				}
				CrearteMaterialUI(modelData);
			});

			foreach (var img in textureList)
			{
				if (img.name == modelData.ModelName)
				{
					var image = modelUI.GetComponentInChildren<RawImage>();
					image.texture = img;
				}
			}
		}
	}

	private void CrearteMaterialUI(modelData modelData)
	{
	

		for (int i = 0; i < modelData.Material.Length; i++)
		{
			var ob = Instantiate(ButtonPrefab, CostumePanel);

			// button
			foreach (var mat in materialList)
			{
				if (mat.name == modelData.Material[i])
				{
					matName = mat.name;	
					var button = ob.GetComponentInChildren<Button>();
					button.GetComponentInChildren<TextMeshProUGUI>().text = mat.name;
					button.onClick.AddListener(() =>
					{
						modelMesh.material = mat;
					});

					break;
				}
			}

			// tex 
			foreach (var img in textureList)
			{
				if (img.name.Contains($"{modelData.ModelName}_{matName}"))
				{
					var image = ob.GetComponentInChildren<RawImage>();
					image.texture = img;

					break;
				}
			}
		}
	}

	private void Destory()
	{
		if (instance != null)
		{
			Destroy(instance);

			for (int i = 0; i < CostumePanel.childCount; i++)
			{
				Destroy(CostumePanel.GetChild(i).gameObject);
			}
		}
	}
}
