// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(CameraFlythough))]
public class CameraFlythoughEditor : Editor {
	
	private CameraFlythough _cameraFlythough;
	GUIStyle style = new GUIStyle();
	
	void OnEnable() {
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white;
		_cameraFlythough = (CameraFlythough)target;
	}
	
	public override void OnInspectorGUI() {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Look At Count");
		_cameraFlythough.LookAtPathCount = Mathf.Max(1, EditorGUILayout.IntField(_cameraFlythough.LookAtPathCount));
		EditorGUILayout.EndHorizontal();
		CameraFlythough.Resize(ref _cameraFlythough.LookAtPath, _cameraFlythough.LookAtPathCount+1);
		
		EditorGUI.indentLevel = 4;
		for(int i=0;i<_cameraFlythough.LookAtPathCount;i++) {
			EditorGUILayout.BeginHorizontal();
			_cameraFlythough.LookAtPath[i] = EditorGUILayout.Vector3Field("Look At Point " + i, _cameraFlythough.LookAtPath[i]);
			EditorGUILayout.EndHorizontal();
		}
		_cameraFlythough.LookAtPath[_cameraFlythough.LookAtPathCount] = _cameraFlythough.LookAtPath[0];
		
		// ============================================================================================================
		
		EditorGUI.indentLevel = 0;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Move Count");
		_cameraFlythough.MovePathCount = Mathf.Max(1, EditorGUILayout.IntField(_cameraFlythough.MovePathCount));
		EditorGUILayout.EndHorizontal();
		CameraFlythough.Resize(ref _cameraFlythough.MovePath, _cameraFlythough.MovePathCount+1);
		
		EditorGUI.indentLevel = 4;		
		for(int i=0;i<_cameraFlythough.MovePathCount;i++) {
			EditorGUILayout.BeginHorizontal();
			_cameraFlythough.MovePath[i] = EditorGUILayout.Vector3Field("Move Point " + i, _cameraFlythough.MovePath[i]);
			EditorGUILayout.EndHorizontal();
		}
		_cameraFlythough.MovePath[_cameraFlythough.MovePathCount] = _cameraFlythough.MovePath[0];
		
		// ============================================================================================================
	
		if(GUI.changed){
			EditorUtility.SetDirty(_cameraFlythough);			
		}
	}
	
	void OnSceneGUI(){		
		if(_cameraFlythough.LookAtPathCount > 0){
			Undo.RecordObject(_cameraFlythough, "Adjust Look A Path");
			for (int i = 0; i < _cameraFlythough.LookAtPathCount; i++) {
				Handles.Label(_cameraFlythough.LookAtPath[i], "LookAt " + i, style);
				_cameraFlythough.LookAtPath[i] = Handles.PositionHandle(_cameraFlythough.LookAtPath[i], Quaternion.identity);
			}	
		}	
		
		if(_cameraFlythough.MovePathCount > 0){
			Undo.RecordObject(_cameraFlythough, "Adjust Move Path");
			for (int i = 0; i < _cameraFlythough.MovePathCount; i++) {
				Handles.Label(_cameraFlythough.MovePath[i], "Move " + i, style);
				_cameraFlythough.MovePath[i] = Handles.PositionHandle(_cameraFlythough.MovePath[i], Quaternion.identity);
			}	
		}
	}

	
}
