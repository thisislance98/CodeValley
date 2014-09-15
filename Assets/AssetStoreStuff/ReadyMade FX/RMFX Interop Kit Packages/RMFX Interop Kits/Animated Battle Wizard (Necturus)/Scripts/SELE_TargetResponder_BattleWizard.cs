//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// SELE_TargetResponder_Constructor
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

[AddComponentMenu("")]

public class SELE_TargetResponder_BattleWizard : BattleWizardResponderBase 
{
	public float scaleModifier = 1.0f;
	
	public override Transform GetConstraintTransform(EffectResponderImplantRMFX implant, string constraintName)
	{
		switch (constraintName)
		{
		case "Anchor":
			return implant.transform.Find(HierarchyPath.BaseAnchor);
		case "CenterAnchor":
			return implant.transform.Find(HierarchyPath.CenterAnchor);
		}

		return null;
	}

	public override bool ModifyEffectParameters(EffectResponderImplantRMFX implant, object paramObject)
	{
		BasicSelectronRMFX.SELE_Parameters p = paramObject as BasicSelectronRMFX.SELE_Parameters;
		if (p != null)
		{
			if (scaleModifier > 0.001f)
				p.globalScale *= scaleModifier;
			return true;
		}

		return false;
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
