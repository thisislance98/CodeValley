//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class ROF_StaffFlame_BattleWizard : ParticleSystemRMFX
{
	protected ROF_CastingFX_BattleWizard owner;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	public override void OnAwake()
	{
		base.OnAwake();
		if (transform.parent)
			owner = transform.parent.GetComponent<ROF_CastingFX_BattleWizard>();
	}

	protected override bool PreLaunchCheck()
	{
		return (owner != null && base.PreLaunchCheck());
	}

	public override void OnUpdate()
	{
		transform.position = owner.staffTip.position;
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
