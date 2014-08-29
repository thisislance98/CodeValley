// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SwarmText))]
public class CycleRandomness : MonoBehaviour {
	public float CycleIntervals;
	
	private SwarmText _stgo;
	private float[] _cycleValues = new float[]{0.03f, 0.03f, 0.5f};
	private float _cycleTimer = 3.0f;
	private int _cycleIndex;
	
	void Start () {
		_stgo = gameObject.GetComponent<SwarmText>();
		_cycleTimer = CycleIntervals;
		_cycleIndex = 0;
	}

	void Update () {
		_cycleTimer -= Time.deltaTime;
		
		int nextIndex = _cycleIndex + 1;
		if (nextIndex >= _cycleValues.Length) nextIndex = 0;
		
		float randomness = Mathf.Lerp(_cycleValues[nextIndex], _cycleValues[_cycleIndex], _cycleTimer);
		_stgo.Randomness = randomness;
		
		if (_cycleTimer <= 0.0f) {
			_cycleTimer = CycleIntervals;
			_cycleIndex++;
			if(_cycleIndex >= _cycleValues.Length) _cycleIndex = 0;
		}
	}
}
