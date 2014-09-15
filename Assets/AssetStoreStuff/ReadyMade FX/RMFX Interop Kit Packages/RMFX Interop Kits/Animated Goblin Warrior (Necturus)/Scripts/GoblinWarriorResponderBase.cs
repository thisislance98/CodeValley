//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Goblin Warrior
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

public abstract class GoblinWarriorResponderBase : EffectResponderRMFX
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
		static public string TalkingWhileGesturing = "G_war_talking";
		static public string DrawMaceFromBelt      = "G_war_weapon_out_01";
		static public string StowWeaponOnBelt      = "G_war_weapon_back_01";
		static public string ReactingToHit         = "G_war_Hit_from_front";
		static public string DigWithMace           = "G_war_Digging";
		static public string DuckUnder             = "G_war_below_high_swing";
		static public string SwingMaceRightToLeft  = "G_war_swing_right";
		static public string ParryRightHanded      = "G_war_parry_left";
		static public string RelaxedIdle           = "G_war_standing";
		static public string CombatIdle            = "G_war_combat_mode";
		static public string FallDownDying         = "G_war_daying";
	}

	static public void DoAnimationClipChecks(Animation anim)
	{
		CheckForClipExistence(anim, ClipNames.TalkingWhileGesturing);
		CheckForClipExistence(anim, ClipNames.DrawMaceFromBelt);
		CheckForClipExistence(anim, ClipNames.StowWeaponOnBelt);
		CheckForClipExistence(anim, ClipNames.ReactingToHit);
		CheckForClipExistence(anim, ClipNames.DigWithMace);
		CheckForClipExistence(anim, ClipNames.DuckUnder);
		CheckForClipExistence(anim, ClipNames.SwingMaceRightToLeft);
		CheckForClipExistence(anim, ClipNames.ParryRightHanded);
		CheckForClipExistence(anim, ClipNames.RelaxedIdle);
		CheckForClipExistence(anim, ClipNames.CombatIdle);
		CheckForClipExistence(anim, ClipNames.FallDownDying);
	}

	private static void CheckForClipExistence(Animation anim, string clipName)
	{
		if (anim.GetClip(clipName) == null)
			Debug.LogWarning("GoblinWarriorResponderBase: Required clip \"" + clipName + "\" does not exist");
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

