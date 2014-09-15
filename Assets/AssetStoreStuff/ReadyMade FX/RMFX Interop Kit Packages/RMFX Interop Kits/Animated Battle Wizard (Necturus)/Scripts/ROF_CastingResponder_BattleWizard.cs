//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class ROF_CastingResponder_BattleWizard : BattleWizardResponderBase
{
	public EffectGroupRMFX castingPrefab;
	public Transform staffTip;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	private class ROF_ClipSeries
	{
		public string clipName;
		public float speed;
		public WrapMode wrapMode;
		public float fade;
	}

	static ROF_ClipSeries[] clipSeries = 
	{
		new ROF_ClipSeries { 
			clipName = ClipNames.LiftStaffSkyward, 
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new ROF_ClipSeries { 
			clipName = ClipNames.PoundGroundWithStaff,
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new ROF_ClipSeries { 
			clipName = ClipNames.PoundGroundWithStaff,
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new ROF_ClipSeries { 
			clipName = ClipNames.DuckUnder,
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new ROF_ClipSeries { 
			clipName = ClipNames.SwingStaffRightToLeft,
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new ROF_ClipSeries { 
			clipName= ClipNames.TwoHandedCombatIdle,
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

	// accumulated duration from all clips in RoF_clipNames[] 
	private float totalAnimDuration = 0.0f;
	private float warmupDuration = 0.0f;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	public override void BeginEffectResponse(EffectResponderImplantRMFX implant, GameObject target, GameObject effect)
	{
		// setup live-data
		liveSpellCaster = implant;
		liveSpellObject = effect;
		
		// find the tip of the staff to use as mount point for flame effect
		if (staffTip == null)
			staffTip = RMFXCore.SearchForChildOf(liveSpellCaster.transform, "staff tip");
		
		if (castingPrefab != null)
		{
			ROF_CastingFX_BattleWizard fx = Instantiate(castingPrefab, liveResponderImplant.transform.position, Quaternion.identity) as ROF_CastingFX_BattleWizard;
			if (fx != null)
			{
				fx.InitEffectGroupInstance(this, "ROF Casting (BattleWizard)", true/*childEffects.hideChildEffects*/);
				fx.staffTip = staffTip;
			}
		}

		// temporarily turn off the character motor
		liveSpellCaster.DisengageCharacterMotor();

		totalAnimDuration = calculateAnimDuration(out warmupDuration);
		if (totalAnimDuration > 0.0f)
			Invoke("CueAnimation_CALLBACK", 0.0f /*animDelay*/);

		if (warmupDuration > 0.0f)
			liveSpellCaster.BeginWarmupPhase(effectName, liveSpellObject, warmupDuration);

		liveSpellCaster.SendMessage("OnWieldWeapon", SendMessageOptions.DontRequireReceiver);

		updateDelegate = Update_Preroll;
	}

	public override void InterruptEffectResponse(EffectResponderImplantRMFX implant) 
	{ 
		if (liveSpellCaster != null)
			liveSpellCaster.EngageCharacterMotor();

		base.InterruptEffectResponse(implant);
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

	private void Update_Preroll()
	{
		if (liveSpellCaster != null && (liveSpellCaster.eventMask & RMFXCore.EventBits.IMPACT) != 0)
		{
			liveSpellObject.SendMessage("OnContinueEffect", SendMessageOptions.DontRequireReceiver);
			updateDelegate = Update_Active;
		}
	}

	private void Update_Active()
	{
		if (elapsedTime > totalAnimDuration)
		{
			liveSpellCaster.SendMessage("OnStowWeapon", SendMessageOptions.DontRequireReceiver);
			liveSpellCaster.EngageCharacterMotor();
			updateDelegate = Update_DoNothing;
		}

		if (warmupDuration > 0.0f && elapsedTime > warmupDuration)
		{
			liveSpellCaster.EndWarmupPhase(effectName, liveSpellObject);
			warmupDuration = 0.0f;
		}
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	private float calculateAnimDuration(out float warmupDuration)
	{
		float animDuration = 0.0f;

		warmupDuration = 0.0f;

		Animation anim = liveSpellCaster.GetComponent<Animation>();
		if (anim != null)
		{
			for (int i = 0; i < clipSeries.Length; i++)
			{
				if (clipSeries[i].clipName == ClipNames.TwoHandedCombatIdle)
					break;

				animDuration += anim[clipSeries[i].clipName].length/clipSeries[i].speed;
				if (i < clipSeries.Length-1)
					animDuration -= clipSeries[i].fade;
			}
		}

		warmupDuration = animDuration - 0.2f;
		animDuration += 0.32f;

		if (warmupDuration < 0.0f)
			warmupDuration = 0.0f;

		return (animDuration > 0.0f) ? animDuration : 0.0f;
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
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
