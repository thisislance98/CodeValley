//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

#if UNITY_EDITOR
// This component uses AnimationUtility to check for AnimationEvents (see below)
using UnityEditor;
#endif

[AddComponentMenu("RMFX Interop/BattleWizard/Battle Wizard Implant")]

public class BattleWizardImplant : EffectResponderImplantRMFX
{
	[System.Serializable]
	public class AnimationMods
	{
		public bool addEffectEvents = true;
		public bool addFootstepEvents = true;
		public bool addDerivedClips = true;
		public bool performChecks = true;
	}

	public AnimationMods animationMods;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	protected override void OnStart()
	{
		Animation anim = GetComponent<Animation>();
		if (anim == null)
			anim = transform.GetComponentInChildren<Animation>();

		if (anim)
		{
			if (animationMods.addEffectEvents)
				AddEffectAnimationEvents(anim);
			if (animationMods.addFootstepEvents)
				AddFootstepAnimationEvents(anim);
			if (animationMods.addDerivedClips)
				AddDerivedAnimationClips(anim);

			if (animationMods.performChecks)
			{
				BattleWizardResponderBase.DoAnimationClipChecks(anim);
#if UNITY_EDITOR
				DoAnimationEventChecks(anim);
#endif
			}
		}
		else
			Debug.LogWarning("AnimatedWizardImplant: No Animation component.");

	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	private static void AddEffectAnimationEvents(Animation anim)
	{
		AnimationClip clip;

		clip = anim.GetClip(BattleWizardResponderBase.ClipNames.HandsMixingPotion);
		if (clip != null)
			AddAnimationEvent(clip, "OnActionEvent", 10);

		clip = anim.GetClip(BattleWizardResponderBase.ClipNames.TwoHandedForwardPush);
		if (clip != null)
			AddAnimationEvent(clip, "OnReleaseEvent", 18);

		clip = anim.GetClip(BattleWizardResponderBase.ClipNames.PoundGroundWithStaff);
		if (clip != null)
			AddAnimationEvent(clip, "OnImpactEvent", 14);
	}

	private static void AddDerivedAnimationClips(Animation anim)
	{
		AnimationClip clip;

		clip = anim.GetClip(BattleWizardResponderBase.ClipNames.ArmsWideSummoning);
		if (clip == null)
		{
			AnimationClip src_clip = anim.GetClip("MM_buff_spell_B");
			if (src_clip != null)
			{
				anim.AddClip(src_clip, BattleWizardResponderBase.ClipNames.ArmsWideSummoning, 51, 90);
			}
		}
	}

	private static void AddFootstepAnimationEvents(Animation anim)
	{
		AnimationClip clip;

		clip = anim.GetClip("MM_walk");
		if (clip != null)
			AddFootstepEvents(clip, 5, 31);

		clip = anim.GetClip("MM_Run");
		if (clip != null)
			AddFootstepEvents(clip, 8, 19);
	}

#if UNITY_EDITOR
	private static void DoAnimationEventChecks(Animation anim)
	{
		CheckClipForAnimationEvent(anim, BattleWizardResponderBase.ClipNames.HandsMixingPotion, "OnActionEvent");
		CheckClipForAnimationEvent(anim, BattleWizardResponderBase.ClipNames.TwoHandedForwardPush, "OnReleaseEvent");
		CheckClipForAnimationEvent(anim, BattleWizardResponderBase.ClipNames.PoundGroundWithStaff, "OnImpactEvent");

		CheckClipForAnimationEvent(anim, "MM_walk", "OnFootstepEvent");
		CheckClipForAnimationEvent(anim, "MM_Run", "OnFootstepEvent");
	}

	private static void CheckClipForAnimationEvent(Animation anim, string clipName, string functionName)
	{
		if (!ClipHasAnimationEvent(anim.GetClip(clipName), functionName))
			Debug.LogWarning("BattleWizardImplant: Clip \"" + clipName + "\" has no AnimationEvents for " + functionName + "().");
	}

	private static bool ClipHasAnimationEvent(AnimationClip clip, string functionName)
	{
		if (clip != null)
		{
			AnimationEvent[] events = AnimationUtility.GetAnimationEvents(clip);
			foreach (AnimationEvent ae in events)
				if (ae.functionName == functionName)
					return true;
		}

		return false;
	}
#endif
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
