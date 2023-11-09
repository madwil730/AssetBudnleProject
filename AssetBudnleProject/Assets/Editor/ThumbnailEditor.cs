using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreateThumbnail))]
public class ThumbnailEditor : Editor
{
	private CreateThumbnail createThumbnail;

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		createThumbnail = (CreateThumbnail)target;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("mesh"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("mat"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("DrawTexture"));

		if (GUILayout.Button("Thumbnail"))
		{
			createThumbnail.CreateThumbnails();
			AssetDatabase.Refresh();
		}

		serializedObject.ApplyModifiedProperties();
	}


}
