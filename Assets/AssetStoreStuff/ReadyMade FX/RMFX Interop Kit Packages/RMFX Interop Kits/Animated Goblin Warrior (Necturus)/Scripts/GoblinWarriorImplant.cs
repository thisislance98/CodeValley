//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Goblin Warrior
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

#if UNITY_EDITOR
// This component uses AnimationUtility to check for AnimationEvents (see below)
using UnityEditor;
#endif

[AddComponentMenu("RMFX Interop/GoblinWarrior/Goblin Warrior Implant")]

public class GoblinWarriorImplant : EffectResponderImplantRMFX
{
	[System.Serializable]
	public class AnimationMods
	{
		public bool addEffectEvents = true;
		public bool addFootstepEvents = true;
		public bool performChecks = true;
	}

	public AnimationMods animationMods;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	protected override void OnStart()
	{
		Animation anim = GetComponent<Animation>();
		if (anim)
		{
			if (animationMods.addEffectEvents)
				AddEffectAnimationEvents(anim);
			if (animationMods.addFootstepEvents)
				AddFootstepAnimationEvents(anim);

			if (animationMods.performChecks)
			{
				GoblinWarriorResponderBase.DoAnimationClipChecks(anim);
#if UNITY_EDITOR
				DoAnimationEventChecks(anim);
#endif
			}
		}		
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	private static void AddEffectAnimationEvents(Animation anim)
	{
		AnimationClip clip;

		clip = anim.GetClip(GoblinWarriorResponderBase.ClipNames.DigWithMace);
		if (clip != null)
			AddAnimationEvent(clip, "OnImpactEvent", 13);
	}

	private static void AddFootstepAnimationEvents(Animation anim)
	{
		AnimationClip clip;

		clip = anim.GetClip("G_war_walk");
		if (clip != null)
			AddFootstepEvents(clip, 4, 21);

		clip = anim.GetClip("G_war_run");
		if (clip != null)
			AddFootstepEvents(clip, 8, 19);
	}

#if UNITY_EDITOR
	private static void DoAnimationEventChecks(Animation anim)
	{
		CheckClipForAnimationEvent(anim, GoblinWarriorResponderBase.ClipNames.DigWithMace, "OnImpactEvent");

		CheckClipForAnimationEvent(anim, "G_war_walk", "OnFootstepEvent");
		CheckClipForAnimationEvent(anim, "G_war_run", "OnFootstepEvent");
	}

	private static void CheckClipForAnimationEvent(Animation anim, string clipName, string functionName)
	{
		if (!ClipHasAnimationEvent(anim.GetClip(clipName), functionName))
			Debug.LogWarning("GoblinWarriorImplant: Clip \"" + clipName + "\" has no AnimationEvents for " + functionName + "().");
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
