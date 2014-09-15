//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Goblin Warrior
// Copyright (C) 2013 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class SOTT_CastingResponder_GoblinWarrior : GoblinWarriorResponderBase
{
	public float verticalScale = 1f;
	public float radialScale = 1f;
	public Vector3 ringCenterOffset = Vector3.zero;

	public float animDelay = 0.0f;
	public float continueDelay = 0.0f;
	public float armMissileDelay = 0.5f;
	public float releaseMissileDelay = 1.0f;
	public float warmupDuration = 2.0f;
	
	public string missileLaunchAnchorNode;
	
	public AnimationClip[] animationClipSeries;
	public float animationSpeed = 1.0f;
	public float animationOverlap = 0.25f;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	private delegate void UpdateDelegate();
	private UpdateDelegate updateDelegate;

	private EffectResponderImplantRMFX liveSpellCaster;
	private GameObject liveSpellObject;

	private float startTime;
	private float elapsedTime;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	public override void BeginEffectResponse(EffectResponderImplantRMFX implant, GameObject target, GameObject effect)
	{
		// setup live-data
		liveSpellCaster = implant;
		liveSpellObject = effect;

		// temporarily turn off the character motor
		liveSpellCaster.DisengageCharacterMotor();

		Invoke("CueAnimation_CALLBACK", animDelay);

		Invoke("Continue_CALLBACK", continueDelay);
		Invoke("ArmMissile_CALLBACK", armMissileDelay);
		Invoke("ReleaseMissile_CALLBACK", releaseMissileDelay);

		if (warmupDuration > 0.0f)
			liveSpellCaster.BeginWarmupPhase(effectName, liveSpellObject, warmupDuration);

		updateDelegate = Update_Active;
	}

	public override Transform GetConstraintTransform(EffectResponderImplantRMFX implant, string constraintName)
	{
		switch (constraintName)
		{
		case "Missile Launch Anchor":
			return implant.transform.Find(HierarchyPath.RightHandMount);
		case "Casting Anchor":
			return implant.transform.Find(HierarchyPath.BaseAnchor);
		}
		
		return null;
	}
	
	public override void InterruptEffectResponse(EffectResponderImplantRMFX implant) 
	{ 
		// restore control to the motor
		if (implant != null)
			implant.EngageCharacterMotor();

		base.InterruptEffectResponse(implant);
	}

	public override bool ModifyEffectParameters(EffectResponderImplantRMFX implant, object paramObject)
	{
		SparksOfTheTempest.SOTT_Parameters p = paramObject as SparksOfTheTempest.SOTT_Parameters;
		if (p != null)
		{
			if (verticalScale > 0.001f)
				p.verticalScale *= verticalScale;
			if (radialScale > 0.001f)
				p.radialScale *= radialScale;
			p.ringCenterOffset += ringCenterOffset;
			
			return true;
		}
		
		return false;
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

	/*
	public float hold_time = 0.0f;
	public float hold_duration = 0.0f;
	private bool hold = false;
	AnimationState hold_animState;
	private float hold_normalizeSpeed;
	*/

	private void Update_Active()
	{
		if (elapsedTime > warmupDuration)
		{
			if (warmupDuration > 0.0f)
				liveSpellCaster.EndWarmupPhase(effectName, liveSpellObject);

			updateDelegate = Update_PostRoll;
		}

		/*
		if( !hold && (elapsedTime > hold_time) )
		{
			hold = true;
			hold_normalizeSpeed = hold_animState.normalizedSpeed;
			hold_time = elapsedTime; 
			hold_animState.speed = 0.0f;
			hold_animState.normalizedSpeed = 0.0f;
			updateDelegate = Update_Hold;
		}
		*/
	}

	/*
	private void Update_Hold()
	{
		if (elapsedTime > hold_time+hold_duration)
		{
			hold_animState.speed = animationSpeed;
			hold_animState.normalizedSpeed = hold_normalizeSpeed;

			updateDelegate = Update_Active;
		}
	}
	*/

	private void Update_PostRoll()
	{
		if (elapsedTime > warmupDuration+0.2f)
		{
			liveSpellCaster.EngageCharacterMotor();
			updateDelegate = Update_DoNothing;
		}
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
	// Invoke-able Callbacks

	void CueAnimation_CALLBACK()
	{
		Animation anim = liveSpellCaster.GetComponent<Animation>();
		if (anim != null)
		{
			AnimationState animState = anim.CrossFadeQueued("G_war_Stretching", .25f, QueueMode.PlayNow);
			animState.speed = 0.8f;
			animState.wrapMode = WrapMode.Once;
		}
	}

	private void Continue_CALLBACK()
	{
		liveSpellObject.SendMessage("OnContinueEffect", SendMessageOptions.DontRequireReceiver);
	}
	
	private void ArmMissile_CALLBACK()
	{
		liveSpellCaster.ManufactureActionEvent();

		Animation anim = liveSpellCaster.GetComponent<Animation>();
		if (anim != null)
		{
			AnimationState animState = anim.CrossFadeQueued("G_war_jump_blow", .25f, QueueMode.PlayNow);
			animState.speed = 1f;
			animState.wrapMode = WrapMode.Once;

			animState = anim.CrossFadeQueued("G_war_combat_mode", .25f, QueueMode.CompleteOthers);
			animState.speed = 1f;
			animState.wrapMode = WrapMode.Once;
		}

	}
	
	private void ReleaseMissile_CALLBACK()
	{
		liveSpellCaster.ManufactureReleaseEvent();
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
