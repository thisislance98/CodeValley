// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using System.Collections;

public class CycleTextColor : MonoBehaviour {
	
	public float CycleIntervals;
	public Color[] CycleColors;
	
	private float _cycleTimer = 2.0f;
	private int _colorIndex;
	
	void Start () {
		_cycleTimer = CycleIntervals;
		_colorIndex = 0;
	}
	
	void Update () {
		_cycleTimer -= Time.deltaTime;
		
		int nextColor = _colorIndex + 1;
		if (nextColor >= CycleColors.Length) nextColor = 0;
		
		Color color = Color.Lerp(CycleColors[nextColor], CycleColors[_colorIndex], _cycleTimer);
		renderer.material.SetColor("_TintColor", color);
		
		if (_cycleTimer <= 0.0f) {
			_cycleTimer = CycleIntervals;
			_colorIndex++;
			if(_colorIndex >= CycleColors.Length) _colorIndex = 0;
		}
	}
}
