//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- Sparks of the Tempest
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;
using System.Collections;

[AddComponentMenu("")]

public class SOTT_SpinWhoosh : MonoBehaviour
{
	public float degreesPerSecond = 30.0f;
	private float startTime;
	
	void Start()
	{
		startTime = Time.time;
	}
	
	void Update()
	{
		Vector3 rot = transform.localEulerAngles;
		rot.y = Mathf.Repeat((Time.time-startTime)*degreesPerSecond, 360.0f); 
		transform.localEulerAngles = rot;
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
