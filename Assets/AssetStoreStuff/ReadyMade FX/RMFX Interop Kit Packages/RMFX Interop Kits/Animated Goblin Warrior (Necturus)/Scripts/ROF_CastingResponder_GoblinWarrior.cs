//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Goblin Warrior
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class ROF_CastingResponder_GoblinWarrior : GoblinWarriorResponderBase
{
	public Transform maceTip;

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
			clipName = ClipNames.DrawMaceFromBelt, 
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new ROF_ClipSeries { 
			clipName = ClipNames.DigWithMace,
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new ROF_ClipSeries { 
			clipName = ClipNames.DigWithMace,
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
			clipName = ClipNames.SwingMaceRightToLeft,
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new ROF_ClipSeries { 
			clipName= ClipNames.CombatIdle,
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
		new ROF_ClipSeries { 
			clipName= ClipNames.StowWeaponOnBelt,
			speed = 1.0f, 
			wrapMode = WrapMode.Once,
			fade = 0.3f,
		},
	};

	static string[] RoF_clipNames = {
		ClipNames.DrawMaceFromBelt, 
		ClipNames.DigWithMace, 
		ClipNames.DigWithMace, 
		ClipNames.DuckUnder, 
		ClipNames.SwingMaceRightToLeft, 
		ClipNames.CombatIdle, 
		ClipNames.StowWeaponOnBelt, 
	};

	private delegate void UpdateDelegate();
	private UpdateDelegate updateDelegate;

	private EffectResponderImplantRMFX liveSpellCaster;
	private GameObject liveSpellObject;

	private float startTime;
	private float elapsedTime;

	private bool needToStowWeapon = false;

	// accumulated duration from all clips in RoF_clipNames[] 
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

		// find the tip of the staff to use as mount point of a flame effect
		if (maceTip == null)
			maceTip = RMFXCore.SearchForChildOf(liveSpellCaster.transform, "mace tip");

		float wieldDelay = 0.0f;

		Animation anim = liveSpellCaster.GetComponent<Animation>();
		if (anim != null)
		{
			float crossFadeLength = 0.3f;

			anim[ClipNames.DigWithMace].wrapMode = WrapMode.Once;

			totalAnimDuration = 0.0f;
			for (int i = 0; i < RoF_clipNames.Length; i++)
			{
				if (i < RoF_clipNames.Length-1)
					totalAnimDuration += anim[RoF_clipNames[i]].length;

				if (RoF_clipNames[i] == ClipNames.DrawMaceFromBelt)
					wieldDelay = totalAnimDuration - 0.65f;

				anim[RoF_clipNames[i]].wrapMode = WrapMode.Once;

				if (i == 0)
					anim.CrossFade(RoF_clipNames[i], crossFadeLength);
				else
					anim.CrossFadeQueued(RoF_clipNames[i], crossFadeLength);
			}

			totalAnimDuration -= crossFadeLength*(RoF_clipNames.Length-1);
		}

		warmupDuration = totalAnimDuration + 0.5f;
		if (warmupDuration > 0.0f)
			liveSpellCaster.BeginWarmupPhase(effectName, liveSpellObject, warmupDuration);

		Invoke("WieldMace_CALLBACK", wieldDelay);

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
		if (elapsedTime > warmupDuration)
		{
			Animation anim = liveSpellCaster.GetComponent<Animation>();
			if (anim != null)
			{
				anim[ClipNames.DigWithMace].wrapMode = WrapMode.Loop;
			}

			if (warmupDuration > 0.0f)
				liveSpellCaster.EndWarmupPhase(effectName, liveSpellObject);

			if (needToStowWeapon)
				liveSpellCaster.SendMessage("OnStowWeapon", SendMessageOptions.DontRequireReceiver);
			liveSpellCaster.EngageCharacterMotor();
			updateDelegate = Update_DoNothing;
		}
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	private bool IsWeaponWielded()
	{
		if (liveSpellCaster != null)
		{
			CharacterGearImplantRMFX gearImplant = liveSpellCaster.GetComponent<CharacterGearImplantRMFX>();
			if (gearImplant != null)
				return gearImplant.IsWeaponWielded();
		}

		return false;
	}

	private float calculateAnimDuration(out float warmupDuration)
	{
		float animDuration = 0.0f;

		warmupDuration = 0.0f;

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

		warmupDuration = animDuration - 0.2f;
		animDuration += 0.32f;

		if (warmupDuration < 0.0f)
			warmupDuration = 0.0f;

		return (animDuration > 0.0f) ? animDuration : 0.0f;
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
	// Invoke-able Callbacks

	public void WieldMace_CALLBACK()
	{
		if (!IsWeaponWielded())
		{
			needToStowWeapon = true;
			liveSpellCaster.SendMessage("OnWieldWeapon", SendMessageOptions.DontRequireReceiver);
		}
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
