//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class EQR_TargetResponder_BattleWizard : BattleWizardResponderBase 
{
	// the settable effect parameters
	public float scaleModifier = 1.0f;
	public float vocalPitch = 1.0f;
	public bool grunting = true;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	public override Transform GetConstraintTransform(EffectResponderImplantRMFX implant, string constraintName)
	{
		switch (constraintName)
		{
		case "Left Foot Anchor":
			return implant.transform.Find(HierarchyPath.LeftFoot);
		case "Right Foot Anchor":
			return implant.transform.Find(HierarchyPath.RightFoot);
		case "Landing Anchor":
			return implant.transform;
		}

		return null;
	}

	public override bool ModifyEffectParameters(EffectResponderImplantRMFX implant, object paramObject)
	{
		EarthquakeRomp.EQR_Parameters p = paramObject as EarthquakeRomp.EQR_Parameters;
		if (p != null)
		{
			if (scaleModifier > 0.001f)
				p.globalScale *= scaleModifier;
			p.vocalPitch = vocalPitch;
			p.grunting = grunting;

			return true;
		}

		return false;
	}

	public override void BeginEffectResponse(EffectResponderImplantRMFX implant, GameObject target, GameObject effect)
	{
		// The default implementation of BeginEffectResponse() in EffectResponderRMFX sends
		// the "OnContinueEffect" message to the effect. Normally this is convenient, but
		// "Earthquake Romp" is unusual in that it calls OnBeginEffect() on both the 
		// spellCaster and the spellTarget passing in the same effect. To prevent sending
		// an extra "OnContinueEffect" message, we override BeginEffectResponse() with this
		// method which does nothing.
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
