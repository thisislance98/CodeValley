// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SwarmText))]
public class TextScore : MonoBehaviour {
	private SwarmText _swarmText;
	private float _score;
	private float[] _randomScoreValues = new float[] {25,50,75,100,150,250,500,1000};
	private float _randomScoreCounter;
	
	void Start () {
		_swarmText = gameObject.GetComponent<SwarmText>();
	}
	
	void Update () {
		_randomScoreCounter -= Time.deltaTime;
		if (_randomScoreCounter <= 0.0f) {
			_randomScoreCounter = 0.75f + Random.Range(0,2);
			addToScore(_randomScoreValues[Random.Range(0, _randomScoreValues.Length)]);
		}
	}
	
	private void addToScore (float scoreToAdd) {
		_score += scoreToAdd;
		_swarmText.Text = "Score:" + _score.ToString();	
	}
	
	
}
