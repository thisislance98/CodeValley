//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- Lightning Strike
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class LST_ThunderClap : AudioSourceRMFX
{
	public AudioClip[] thunderClips;
	
	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	protected override void InitParameters() 
	{ 
		audio.clip = thunderClips[ Random.Range(0, thunderClips.Length) ];
		audio.pitch = Random.Range(.8f, 1f);
		audio.volume = Random.Range(.8f, 1f);
	}

	protected override bool PreLaunchCheck()
	{
		return (thunderClips.Length > 0 && base.PreLaunchCheck());
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//