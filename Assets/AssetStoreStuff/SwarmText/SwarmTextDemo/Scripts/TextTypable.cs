// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SwarmText))]
public class TextTypable : MonoBehaviour {
	public Texture2D[] Fonts;
	
	private SwarmText _stgo;
	
	void Start () {	
		_stgo = gameObject.GetComponent<SwarmText>();
	}
	
	void Update () {
	}
	
	void OnGUI () {
		GUI.Label(new Rect(10,330,200,20), "Type Here:");
		_stgo.Text = GUI.TextField(new Rect(80, 330, 280, 20), _stgo.Text, 12);
		
		for(int i=0;i<Fonts.Length;i++) {
			if (GUI.Button(new Rect(80 + (i*47), 355, 45, 20), "Font" + (i+1))) 
				_stgo.FontTexture = Fonts[i];
		}
	}
}
