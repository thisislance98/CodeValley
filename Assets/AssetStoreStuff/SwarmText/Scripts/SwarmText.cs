// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SwarmText : MonoBehaviour {

	public string Text = "Hello World!";


	public float FontSize;

	public TextAnchor AnchorPoint = TextAnchor.LowerLeft;
	public Texture2D FontTexture;
	public float SpinSpeed = 0.0f;
	
	public float Population = 0.3f;
	public float Randomness = 0.03f;
	public float Speed = 3.0f;

	private SwarmString _swarmString;
	private ParticleEmitter _particleEmitter;
	private ParticleRenderer _particleRenderer;

	public 
	
	void Start () {
		_particleEmitter = gameObject.particleEmitter;
		_particleRenderer = gameObject.GetComponent<ParticleRenderer>();
		EmitParticles(false);
	}
	
	void Update () {
		// Check for change and update string
		if (_swarmString.GetText() != Text 
		    || _swarmString.GetFontSize() != FontSize 
		    || _swarmString.GetPopulation() != Population
		    || _swarmString.GetAnchorPoint() != AnchorPoint
		    || _swarmString.GetFontTexture() != FontTexture) {
			EmitParticles(false);
		}
		
		// Move and update agents
		Particle[] particles = _particleEmitter.particles;
		SwarmAgent[] agents = _swarmString.GetAgents();
		OrbitEffect.Update(ref particles, agents, Speed, Randomness);
		_particleEmitter.particles = particles;
	}
	
	void OnDrawGizmos () {
		// Check for change and update string
		if (!Application.isPlaying) {
			if (_swarmString == null
			    || _particleEmitter == null
			    || _particleEmitter.particleCount <= 0
			    || _swarmString.GetText() != Text 
			    || _swarmString.GetFontSize() != FontSize 
			    || _swarmString.GetPopulation() != Population
			    || _swarmString.GetAnchorPoint() != AnchorPoint
			    || _swarmString.GetFontTexture() != FontTexture) {
				_particleEmitter = gameObject.particleEmitter;
				_particleRenderer = gameObject.GetComponent<ParticleRenderer>();
				EmitParticles(true);
			}
		}
	}
	
	public void EmitParticles (bool showLetter) {
		if (_swarmString == null || _swarmString.GetFontTexture() != FontTexture) {
			_swarmString = new SwarmString(Text, Population, FontTexture, FontSize, AnchorPoint, SpinSpeed);
		} else {
			_swarmString.ChangeText(Text, FontSize, Population, AnchorPoint);
		}
		
		SwarmAgent[] agents = _swarmString.GetAgents();
		
		int startParticleCount = _particleEmitter.particles.Length;
		if (_particleEmitter.particles.Length < agents.Length) {
		    _particleEmitter.Emit(agents.Length - _particleEmitter.particles.Length);
		}
		
		Particle[] particles = _particleEmitter.particles;
			
		for(int i=0; i<agents.Length;i++) {
			if (showLetter)
				particles[i].position = agents[i].TargetPosition;
			else if (i >= startParticleCount)
				particles[i].position = agents[i].CenterPoint;
			
			if (_particleRenderer.uvAnimationXTile == 1 && _particleRenderer.uvAnimationYTile == 1)
				particles[i].energy = 1;
			else if (i >= startParticleCount)
				particles[i].energy = Random.value*((_particleRenderer.uvAnimationXTile*_particleRenderer.uvAnimationYTile)-1);
		}
		
		if (particles.Length > agents.Length) {
			for(int i=agents.Length; i < particles.Length; i++) {
				particles[i].energy = -1;
			}
		}
				
		_particleEmitter.particles = particles;
	} 
			
	// ========================================================================
	
	public class SwarmString {
		private string _text;
		private float _population;
		private Texture2D _fontTexture;
		private float _fontSize;
		private TextAnchor _anchorPoint;
		private float _spinSpeed;
		
		private SwarmAgent[] _agents;
		private List<SwarmLetter> _letters = new List<SwarmLetter>();
		
		public SwarmString(string text, float population, Texture2D fontTexture, float fontSize, TextAnchor anchorPoint, float spinSpeed) {
			_text = text;
			_population = population;
			_fontTexture = fontTexture;
			_fontSize = fontSize;
			_anchorPoint = anchorPoint;
			_spinSpeed = spinSpeed;
			build();
		}
		
		public void ChangeText(string text, float fontSize, float population, TextAnchor anchorPoint) {
			_fontSize = fontSize;
			_anchorPoint = anchorPoint;
			
			Vector3 letterPosition = Vector3.zero;
			for(int i=0; i<text.Length; i++) {
				if (_text.Length > i && _population == population) {
					if (text[i] == _text[i]) {
						_letters[i].Scale(_fontSize);
						_letters[i].Offset(letterPosition);
						letterPosition += (Vector3.right * (FontHelper.GetLetterWidth(_text[i], _fontTexture) * _fontSize));
						continue;
					}
				}
				
				SwarmLetter letter = new SwarmLetter(text[i], _population, _fontTexture, _fontSize, _spinSpeed);
				letter.Offset(letterPosition);
				
				if (_letters.Count > i) {
					_letters[i] = letter;
				} else {
					_letters.Add(letter);
				}
				
				letterPosition += (Vector3.right * (FontHelper.GetLetterWidth(text[i], _fontTexture)* _fontSize));
			}
			
			if (_letters.Count > text.Length) {
				_letters.RemoveRange(text.Length, _letters.Count - text.Length);
			}
			
			_population = population;
			_text = text;
			
			applyAnchor(_fontSize, letterPosition);
			List<SwarmAgent> agents = new List<SwarmAgent>();
			foreach(SwarmLetter pl in _letters) {
				agents.AddRange(pl.GetAgents());
			}
			
			_agents = agents.ToArray();
		}
		
		public string GetText() {
			return _text;
		}
		
		public float GetFontSize() {
			return _fontSize;
		}
		
		public float GetPopulation() {
			return _population;
		}
		
		public TextAnchor GetAnchorPoint() {
			return _anchorPoint;
		}
		
		public Texture2D GetFontTexture () {
			return _fontTexture;
		}
		
		private void build() {
			_letters.Clear();
			Vector3 letterPosition = Vector3.zero;
			
			for(int i=0; i<_text.Length; i++) {
				SwarmLetter letter = new SwarmLetter(_text[i], _population, _fontTexture, _fontSize, _spinSpeed);
				letter.Offset(letterPosition);
				_letters.Add(letter);
				letterPosition += (Vector3.right * (FontHelper.GetLetterWidth(_text[i], _fontTexture) * _fontSize));
			}
			
			applyAnchor(_fontSize, letterPosition);
			List<SwarmAgent> agents = new List<SwarmAgent>();
			foreach(SwarmLetter letter in _letters) {
				agents.AddRange(letter.GetAgents());
			}
			
			_agents = agents.ToArray();
		}
		
		private void applyAnchor(float stringHeight, Vector3 stringWidth) {
			switch(_anchorPoint) {
			case TextAnchor.LowerCenter:
				offset(-stringWidth/2);
				break;
			case TextAnchor.LowerLeft:
				// Default Position
				break;
			case TextAnchor.LowerRight:
				offset(-stringWidth);
				break;
			case TextAnchor.MiddleCenter:
				offset(-stringWidth/2 + (Vector3.down * (stringHeight/2)));
				break;
			case TextAnchor.MiddleLeft:
				offset(Vector3.down * (stringHeight/2));
				break;
			case TextAnchor.MiddleRight:
				offset(-stringWidth + (Vector3.down * (stringHeight/2)));
				break;
			case TextAnchor.UpperCenter:
				offset(-stringWidth/2 + (Vector3.down * stringHeight));
				break;
			case TextAnchor.UpperLeft:
				offset(Vector3.down * stringHeight);
				break;
			case TextAnchor.UpperRight:
				offset(-stringWidth + (Vector3.down * stringHeight));
				break;
			}
		}
		
		private void offset(Vector3 offsetBy) {
			foreach(SwarmLetter letter in _letters) {
				letter.Offset(offsetBy);
			}
		}
		
		public SwarmAgent[] GetAgents() {
			return _agents;
		}
	}
	
	// ========================================================================
	
	public class SwarmLetter {
		private char _letter;
		private float _population;
		private Texture2D _fontTexture;
		private float _fontSize;
		private float _spinSpeed;
		
		private SwarmAgent[] _agents;
		
		public SwarmLetter(char letter, float population, Texture2D fontTexture, float fontSize, float spinSpeed) {
			_letter = letter;
			_population = population;
			_fontTexture = fontTexture;
			_fontSize = fontSize;
			_spinSpeed = spinSpeed;
			build();
		}
		
		public void Offset(Vector3 dist) {
			for (int i=0; i < _agents.Length; i++) {
				_agents[i].TargetPosition += dist;
				_agents[i].CenterPoint += dist;
			}
		}
		
		private void build() {	
			List<Vector3> fontPoints = new List<Vector3>(FontHelper.GetLetterPoints(_letter, _fontTexture));
			float centerX = FontHelper.GetLetterWidth(_letter, _fontTexture)/2;
			
			_agents = new SwarmAgent[(int)(fontPoints.Count * _population)];
			
			for (int i=0; i < _agents.Length; i++) {
				int r =  Random.Range(0, fontPoints.Count);
				Vector3 dest = fontPoints[r] * 0.01f;
				fontPoints.RemoveAt(r);
				
				_agents[i] = new SwarmAgent(dest, new Vector3(centerX,0.5f,0), _fontSize, _spinSpeed);
			}		
		}
		
		public SwarmAgent[] GetAgents() {
			return _agents;
		}
		
		public void Scale(float fontSize) {
			foreach(SwarmAgent agent in _agents) {
				agent.Scale(fontSize);
			}
		}
	}
	
	// ========================================================================
	
	public class SwarmAgent {
		public Vector3	TargetPosition;
		public Vector3	Rotation;
		public Vector3	CenterPoint;
		public float 	SpinSpeed;
		
		private Vector3 _unscaledTargetPosition;
		private Vector3 _unscaledCenterPoint;
		
		public SwarmAgent (Vector3 targetPosition, Vector3 centerPoint, float fontSize, float spinSpeed) {
			TargetPosition = targetPosition * fontSize;
			Rotation = Random.insideUnitSphere;
			CenterPoint = centerPoint * fontSize;
			SpinSpeed = -(spinSpeed * 2 * Random.value) + spinSpeed; // -spinSpeed to spinSpeed
			
			_unscaledTargetPosition = targetPosition;
			_unscaledCenterPoint = centerPoint;
		}
		
		public void Scale(float fontSize) {
			TargetPosition = _unscaledTargetPosition * fontSize;
			CenterPoint = _unscaledCenterPoint * fontSize;
		}
	}
	
	// ========================================================================
	
	public class OrbitEffect {
		public static void Update(ref Particle[] particles, SwarmAgent[] agents, float speed, float randomness) {
			for (int i=0; i < particles.Length; i++) {
				agents[i].Rotation.x += Random.Range(0,360) * Time.deltaTime; if (agents[i].Rotation.x > 360) agents[i].Rotation.x -= 360;
				agents[i].Rotation.y += Random.Range(0,360) * Time.deltaTime; if (agents[i].Rotation.y > 360) agents[i].Rotation.y -= 360;
				
				Vector3 target = agents[i].TargetPosition;
				target += Quaternion.Euler(agents[i].Rotation) * new Vector3(0,0,randomness);		
				
				Vector3 difference = particles[i].position - target;
				float distance = difference.magnitude - ((difference.magnitude*speed) * Time.deltaTime);
				particles[i].position = (difference.normalized * distance) + target;
				
				particles[i].rotation += agents[i].SpinSpeed * Time.deltaTime * 100;
			}
		}
	}
	
	// ========================================================================
}