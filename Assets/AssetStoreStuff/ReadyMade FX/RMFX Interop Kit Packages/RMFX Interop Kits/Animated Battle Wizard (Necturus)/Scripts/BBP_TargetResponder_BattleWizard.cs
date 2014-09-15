//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class BBP_TargetResponder_BattleWizard : BattleWizardResponderBase
{
	[System.Serializable]
	public class FootstepProperties
	{
		public BBP_StepFX footstepFX;
		public Vector3 footPositionOffset;
		public Vector3 footAnglesOffset;
		[System.NonSerialized]
		public Transform footAnchor;
		[System.NonSerialized]
		public Quaternion footRotationOffset;
	}

	public FootstepProperties leftFootProperties;
	public FootstepProperties rightFootProperties;

	public override bool IsValidEffectResponder(EffectResponderImplantRMFX implant, GameObject target) 
	{ 
		return implant.GetActiveEffectCountByName(effectName) == 0; 
	}

	public override Object GetExtendedConstraintData(EffectResponderImplantRMFX implant, string constraintName)
	{
		BasicBootprints.ExtendedConstraintData data = null;

		switch (constraintName)
		{
		case "Left Foot":
			data = new BasicBootprints.ExtendedConstraintData();
			data.footstepFX = leftFootProperties.footstepFX;
			data.footAnchor = implant.transform.Find(HierarchyPath.LeftFootToe);
			data.footPositionOffset = leftFootProperties.footPositionOffset;
			data.footRotationOffset = Quaternion.Euler(leftFootProperties.footAnglesOffset);
			break;
		case "Right Foot":
			data = new BasicBootprints.ExtendedConstraintData();
			data.footstepFX = rightFootProperties.footstepFX;
			data.footAnchor = implant.transform.Find(HierarchyPath.RightFootToe);
			data.footPositionOffset = rightFootProperties.footPositionOffset;
			data.footRotationOffset = Quaternion.Euler(rightFootProperties.footAnglesOffset);
			break;
		}

		return data;
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
