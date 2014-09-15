//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;
using System.Collections;

[AddComponentMenu("")]

public class ROF_CastingFX_BattleWizard : EffectGroupRMFX 
{
	public Transform staffTip;
	
	[System.NonSerialized]
	public bool projectorZodiacs = true;
	[System.NonSerialized]
	public bool planarZodiacs = true;
	[System.NonSerialized]
	public bool lights = true;
	[System.NonSerialized]
	public GameObject spellCaster;
	[System.NonSerialized]
	public float ringRadius;
	[System.NonSerialized]
	public float ringDuration;
	[System.NonSerialized]
	public ROF_FireCluster fireClusterPrefab;
	[System.NonSerialized]
	public NamedEffectRMFX burnEffectPrefab;
	[System.NonSerialized]
	public Vector3 groundNormal = Vector3.up;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	public virtual void SetParameters(RingOfFire.ROF_Parameters parameters)
	{
		spellCaster = parameters.spellCaster;
		ringRadius = parameters.ringRadius;
		ringDuration = parameters.ringDuration;
		fireClusterPrefab = parameters.fireClusterPrefab;
		burnEffectPrefab = parameters.burnEffectPrefab;
		projectorZodiacs = parameters.projectorZodiacs;
		planarZodiacs = parameters.planarZodiacs;
		lights = parameters.lights;
		groundNormal = parameters.groundNormal;
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
