//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class BCHAR_Responder_BattleWizard : BattleWizardResponderBase 
{
	public override Transform GetConstraintTransform(EffectResponderImplantRMFX implant, string constraintName)
	{
		switch (constraintName)
		{
		case "Base Anchor":
			return implant.transform.Find(HierarchyPath.BaseAnchor);
		case "Left Hand Mount":
			return implant.transform.Find(HierarchyPath.LeftHandMount);
		case "Right Hand Mount":
			return implant.transform.Find(HierarchyPath.RightHandMount);
		case "Left Foot Anchor":
			return implant.transform.Find(HierarchyPath.LeftFoot);
		case "Right Foot Anchor":
			return implant.transform.Find(HierarchyPath.RightFoot);
		case "Mid Body Anchor":
			return implant.transform.Find(HierarchyPath.Spine2);
		}

		return null;
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
