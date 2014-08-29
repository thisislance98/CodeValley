// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SwarmText))]
public class SwarmTextEditor : Editor {

	private SwarmText _swarmText;
	
	void OnEnable () {
		_swarmText = (SwarmText)target;
	}
	
//	public override void OnInspectorGUI () {
//
//		
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Text");
//		_swarmText.Text = EditorGUILayout.TextField(_swarmText.Text);
//		EditorGUILayout.EndHorizontal();
//		
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Text");
//		_swarmText.AnchorPoint = (TextAnchor)EditorGUILayout.EnumPopup(_swarmText.AnchorPoint);
//		EditorGUILayout.EndHorizontal();
//		
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Font Texture");
//		_swarmText.FontTexture = (Texture2D)EditorGUILayout.ObjectField(_swarmText.FontTexture, typeof(Texture2D), false);
//		EditorGUILayout.EndHorizontal();
//
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Population");
//		_swarmText.Population = EditorGUILayout.Slider(_swarmText.Population, 0.03f, 1.0f);
//		EditorGUILayout.EndHorizontal();
//		
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Speed");
//		_swarmText.Speed = EditorGUILayout.Slider(_swarmText.Speed, 0.5f, 20.0f);
//		EditorGUILayout.EndHorizontal();
//		
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Randomness");
//		_swarmText.Randomness = EditorGUILayout.Slider(_swarmText.Randomness, 0.001f, 1.0f);
//		EditorGUILayout.EndHorizontal();
//		
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Font Size");
//		_swarmText.FontSize = EditorGUILayout.Slider(_swarmText.FontSize, 0.1f, 10.0f);
//		EditorGUILayout.EndHorizontal();
//		
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.PrefixLabel("Spin Speed");
//		_swarmText.SpinSpeed = EditorGUILayout.Slider(_swarmText.SpinSpeed, 0.0f, 10.0f);
//		EditorGUILayout.EndHorizontal();
//	}
	
	void OnSceneGUI () {
	}
}
