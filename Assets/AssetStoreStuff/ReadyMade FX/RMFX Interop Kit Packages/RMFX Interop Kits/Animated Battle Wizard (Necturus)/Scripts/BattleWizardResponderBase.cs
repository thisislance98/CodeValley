//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

public abstract class BattleWizardResponderBase : EffectResponderRMFX
{
	public class HierarchyPath
	{
		static public string BaseAnchor     = "";
		static public string MidBody        = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2";
		static public string CenterAnchor   = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2";
		static public string Spine2         = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2";
		static public string LeftFoot       = "Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot";
		static public string LeftFootToe    = "Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe0";
		static public string RightFoot      = "Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot";
		static public string RightFootToe   = "Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe0";
		static public string RightHandMount = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R ForeTwist/Bip01 R ForeTwist1/Bip01 R Hand/Bip01 Rhand_Weapon";
		static public string LeftHandMount  = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L ForeTwist/Bip01 L ForeTwist1/Bip01 L Hand";
	}

	public class ClipNames
	{
		static public string DefaultCombatIdle = "MM_combat_mode_C";
		static public string ArmsWideSummoning = "MM_summoning+";
		static public string HandsMixingPotion = "MM_making_potion";
		static public string TwoHandedForwardPush = "MM_spell_cast_B";
		static public string ReactingToHit = "MM_taking_hit";
		static public string FallDownDying = "MM_Dying_A";
		static public string LiftStaffSkyward = "MM_magic_light_spell";
		static public string PoundGroundWithStaff = "MM_staff_earthquake_spell";
		static public string DuckUnder = "MM_duck_below_swing";
		static public string SwingStaffRightToLeft = "MM_swing_right";
		static public string TwoHandedCombatIdle = "MM_combat_mode_A";
	}

	static public void DoAnimationClipChecks(Animation anim)
	{
		CheckForClipExistence(anim, ClipNames.DefaultCombatIdle);
		CheckForClipExistence(anim, ClipNames.ArmsWideSummoning);
		CheckForClipExistence(anim, ClipNames.HandsMixingPotion);
		CheckForClipExistence(anim, ClipNames.TwoHandedForwardPush);
		CheckForClipExistence(anim, ClipNames.ReactingToHit);
		CheckForClipExistence(anim, ClipNames.FallDownDying);
		CheckForClipExistence(anim, ClipNames.LiftStaffSkyward);
		CheckForClipExistence(anim, ClipNames.PoundGroundWithStaff);
		CheckForClipExistence(anim, ClipNames.DuckUnder);
		CheckForClipExistence(anim, ClipNames.SwingStaffRightToLeft);
		CheckForClipExistence(anim, ClipNames.TwoHandedCombatIdle);
	}

	private static void CheckForClipExistence(Animation anim, string clipName)
	{
		if (anim.GetClip(clipName) == null)
			Debug.LogWarning("BattleWizardResponderBase: Required clip \"" + clipName + "\" does not exist");
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

