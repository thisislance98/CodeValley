// Version 1.0
// ©2011 Happymess Software. All rights reserved. Redistribution of source code without permission not allowed.
// Created By: JohnKnaack@gmail.com

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFlythough : MonoBehaviour {
	public int MovePathCount;
	public int LookAtPathCount;
	public List<Vector3> MovePath = new List<Vector3>();
	public List<Vector3> LookAtPath = new List<Vector3>();
	
	private float _cycleIntervals = 16.0f;
	private float _flyTime = 8.0f;
	private float _cycleTimer;
	private float _percentage;
	
	void Start () {
		_cycleTimer = _cycleIntervals/2;
	}
	
	void Update () {
		iTween.PutOnPath(gameObject,MovePath.ToArray(), _percentage);
		transform.LookAt(iTween.PointOnPath(LookAtPath.ToArray(),_percentage));
		
		_cycleTimer -= Time.deltaTime;
		if (_cycleTimer <= 0.0f) {
			_cycleTimer = _cycleIntervals;
			_percentage = 0.0f;
			iTween.Stop(gameObject);
			iTween.ValueTo(gameObject,iTween.Hash("from",_percentage,"to",1.0f,"time",_flyTime,"easetype",iTween.EaseType.easeInOutSine,"onupdate","SlidePercentage"));
		}
	}
	
	void SlidePercentage(float p){
		_percentage = p;
	}
	
	void OnDrawGizmosSelected(){
		if(LookAtPath.Count > 1){
			iTween.DrawPath(LookAtPath.ToArray(), Color.green);
		}	
		
		if(MovePath.Count > 1){
			iTween.DrawPath(MovePath.ToArray(), Color.blue);
		}
	}
	
	public static void Resize(ref List<Vector3> arr, int count) {		
		// Add
		if(count > arr.Count){
			for (int i = 0; i < count - arr.Count; i++) {
				arr.Add(new Vector3());	
			}
		}
		// Remove
		if(count < arr.Count){
			int removeCount = arr.Count - count;
			arr.RemoveRange(arr.Count-removeCount,removeCount);
		}
	}
}
