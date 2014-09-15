//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Goblin Warrior
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class FB_CastingResponder_GoblinWarrior : GoblinWarriorResponderBase
{
	public float animDelay = 0.0f;
	public float continueDelay = 0.0f;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	private class FB_ClipSeries
	{
		public string clipName;
		public float speed;
		public WrapMode wrapMode;
		public float fade;
	}

	static FB_ClipSeries[] clipSeries = 
	{
		new FB_ClipSeries { 
			clipName = ClipNames.TalkingWhileGesturing, 
			speed = 0.8f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new FB_ClipSeries { 
			clipName = ClipNames.SwingMaceRightToLeft, 
			speed = 0.7f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new FB_ClipSeries { 
			clipName = ClipNames.RelaxedIdle, 
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
	};

	private delegate void UpdateDelegate();
	private UpdateDelegate updateDelegate;

	private EffectResponderImplantRMFX liveSpellCaster;
	private GameObject liveSpellObject;

	private float startTime;
	private float elapsedTime;

	// accumulated duration from all clips in FB_clipNames[] 
	private float totalAnimDuration = 0.0f;
	private float warmupDuration = 0.0f;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	public override void BeginEffectResponse(EffectResponderImplantRMFX implant, GameObject target, GameObject effect)
	{
		// setup live-data
		liveSpellCaster = implant;
		liveSpellObject = effect;

		// temporarily turn off the character motor
		liveSpellCaster.DisengageCharacterMotor();

		totalAnimDuration = calculateAnimDuration();
		if (totalAnimDuration > 0.0f)
			Invoke("CueAnimation_CALLBACK", animDelay);

		warmupDuration = totalAnimDuration + animDelay;
		if (warmupDuration > 0.0f)
			liveSpellCaster.BeginWarmupPhase(effectName, liveSpellObject, warmupDuration);

		Invoke("Continue_CALLBACK", continueDelay);

		Invoke("igniteFireball_CALLBACK", 1.8f);
		Invoke("releaseFireball_CALLBACK", 2.18f);

		updateDelegate = Update_Active;
	}

	public override void InterruptEffectResponse(EffectResponderImplantRMFX implant) 
	{ 
		if (liveSpellCaster != null)
			liveSpellCaster.EngageCharacterMotor();

		base.InterruptEffectResponse(implant);
	}

	public override Transform GetConstraintTransform(EffectResponderImplantRMFX implant, string constraintName)
	{
		switch (constraintName)
		{
		case "FireBall Anchor":
			return implant.transform.Find(HierarchyPath.RightHandMount);
		}

		return null;
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	protected override void Start()
	{ 
		startTime = Time.time;
	}

	void LateUpdate()
	{
		elapsedTime = Time.time - startTime;
		updateDelegate();
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
	// Update Delegates

	private void Update_DoNothing() { }


	private void Update_Active()
	{
		if (elapsedTime > warmupDuration)
		{
			if (warmupDuration > 0.0f)
				liveSpellCaster.EndWarmupPhase(effectName, liveSpellObject);

			liveSpellCaster.EngageCharacterMotor();
			updateDelegate = Update_DoNothing;
		}
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	private float calculateAnimDuration()
	{
		float animDuration = 0.0f;

		Animation anim = liveSpellCaster.GetComponent<Animation>();
		if (anim != null)
		{
			for (int i = 0; i < clipSeries.Length; i++)
			{
				animDuration += anim[clipSeries[i].clipName].length/clipSeries[i].speed;
				if (i < clipSeries.Length-1)
					animDuration -= clipSeries[i].fade;
			}
		}

		return Mathf.Max(0.0f, animDuration);
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
	// Invoke-able Callbacks

	void CueAnimation_CALLBACK()
	{
		Animation anim = liveSpellCaster.GetComponent<Animation>();
		if (anim != null)
		{
			for (int i = 0; i < clipSeries.Length; i++)
			{
				AnimationState animState = anim.CrossFadeQueued(clipSeries[i].clipName, clipSeries[i].fade, (i == 0) ? QueueMode.PlayNow : QueueMode.CompleteOthers);
				animState.speed = clipSeries[i].speed;
				animState.wrapMode = clipSeries[i].wrapMode;
			}
		}
	}

	private void Continue_CALLBACK()
	{
		liveSpellObject.SendMessage("OnContinueEffect", SendMessageOptions.DontRequireReceiver);
	}

	private void igniteFireball_CALLBACK()
	{
		liveSpellCaster.ManufactureActionEvent();
	}

	private void releaseFireball_CALLBACK()
	{
		liveSpellCaster.ManufactureReleaseEvent();
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
