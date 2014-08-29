// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SwarmText))]
public class TextFPS : MonoBehaviour {
	private SwarmText _swarmText;
    private float _currentFps;
    private float _cycles;

    private string FPSText {
        get { return "FPS: " + _currentFps; }
    }

    private void Start() {
		_swarmText = gameObject.GetComponent<SwarmText>();
    }

    private void Update() {
        if (_cycles >= _currentFps) {
            _currentFps = 1/Time.deltaTime;
            _cycles = 0;
			_swarmText.Text = "FPS: " + Mathf.FloorToInt(_currentFps);
        } else {
            _cycles++;
        }
    }
}
