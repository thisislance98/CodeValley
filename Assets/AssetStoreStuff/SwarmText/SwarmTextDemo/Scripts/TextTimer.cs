// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SwarmText))]
public class TextTimer : MonoBehaviour {
	private bool _timerRunning = true;
	private float _timerCounter;
	private SwarmText _stgo;
	
	void Start () {
		_stgo = gameObject.GetComponent<SwarmText>();
	}
	
	void Update () {
		if (_timerRunning)
			_timerCounter += Time.deltaTime;
			
		int seconds = Mathf.CeilToInt(_timerCounter);
		_stgo.Text = string.Format("{0:00}:{1:00}", (int)(seconds/60f), (int)(seconds%60f));
	}
}
