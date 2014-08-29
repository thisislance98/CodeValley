// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SwarmText))]
public class TextLoading : MonoBehaviour {
	
	public SwarmText[] ObjectToEnable;
	
	private LetterAtTime[] _letters;// = new LetterAtTime[] {
//		new LetterAtTime('L', 0.2f),
//		new LetterAtTime('o', 0.4f),
//		new LetterAtTime('a', 0.6f),
//		new LetterAtTime('d', 0.8f),
//		new LetterAtTime('i', 1.0f),
//		new LetterAtTime('n', 1.2f),
//		new LetterAtTime('g', 1.4f),
//		new LetterAtTime('.', 1.9f),
//		new LetterAtTime('.', 2.4f),
//		new LetterAtTime('.', 2.9f),
//		new LetterAtTime(' ', 3.4f)
//	};
//	
	private float _timer;
	private SwarmText _swarmText;
	
	void Start () {
		PopulateLetters();

		for(int i=0; i<ObjectToEnable.Length;i++) {
			ObjectToEnable[i].gameObject.SetActive(false);
		}
		_swarmText = gameObject.GetComponent<SwarmText>();
		_swarmText.Text = "";
	}

	void PopulateLetters()
	{
		string text = GetComponent<SwarmText>().Text;

		List<LetterAtTime> letters = new List<LetterAtTime>();
		float time=1;
		float timeBetweenLetters = .4f;

		for (float i=0; i < text.Length; i++)
		{
			letters.Add (new LetterAtTime((char)text[(int)i],time));
			time += timeBetweenLetters;

		}
		_letters = letters.ToArray();
	}

	void Update () {
		_timer += Time.deltaTime;
		bool pending = false;
		for(int i=0;i<_letters.Length;i++) {
			if (!_letters[i].Displayed) {
				if (_letters[i].AtTime < _timer) {
					_swarmText.Text += _letters[i].Letter;
					_letters[i].Displayed = true;
				} else {
					pending = true;
				}
			}
		}
		
		if (!pending) {
			for(int i=0; i<ObjectToEnable.Length;i++) {
				ObjectToEnable[i].gameObject.SetActive(true); 
			}
//			gameObject.SetActive(false);
		}
	}
	
	private struct LetterAtTime {
		public char Letter;
		public float AtTime;
		public bool Displayed;
		
		public LetterAtTime(char letter, float atTime) {
			Letter = letter;
			AtTime = atTime;
			Displayed = false;
		}
	}
}