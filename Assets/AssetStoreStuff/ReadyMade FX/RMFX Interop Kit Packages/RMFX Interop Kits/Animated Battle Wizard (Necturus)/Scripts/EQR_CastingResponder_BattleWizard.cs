//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class EQR_CastingResponder_BattleWizard : BattleWizardResponderBase
{
	private delegate void UpdateDelegate();
	private UpdateDelegate updateDelegate;

	private EffectResponderImplantRMFX liveSpellCaster;
	private GameObject liveSpellObject;

	private float startTime;
	private float elapsedTime;

	private float totalDuration = 0.0f;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	public override void BeginEffectResponse(EffectResponderImplantRMFX implant, GameObject target, GameObject effect)
	{
		liveSpellCaster = implant;
		liveSpellObject = effect;

		liveSpellCaster.DisengageCharacterMotor();

		Animation anim = liveSpellCaster.GetComponent<Animation>();
		if (anim != null)
		{
			AnimationState animState = anim.CrossFadeQueued(ClipNames.ArmsWideSummoning, 0.3f, QueueMode.PlayNow);
			totalDuration = (39.0f/30.0f) - 0.3f; //animState.length;
			animState.wrapMode = WrapMode.Once;

			Invoke("SummoningCut_CALLBACK", totalDuration);
		}

		Invoke("Continue_CALLBACK", 0.36f);

		updateDelegate = Update_Active;
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	protected override void Start()
	{ 
		startTime = Time.time;
		if (updateDelegate == null)
			updateDelegate = Update_DoNothing;
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
		if (elapsedTime > totalDuration + 0.2f)
		{
			liveSpellCaster.EngageCharacterMotor();
			updateDelegate = Update_DoNothing;
		}
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
	// Invoke-able Callbacks

	private void Continue_CALLBACK()
	{
		liveSpellObject.SendMessage("OnContinueEffect", SendMessageOptions.DontRequireReceiver);
	}

	private void SummoningCut_CALLBACK()
	{
		Animation anim = liveSpellCaster.GetComponent<Animation>();
		if (anim != null)
		{
			AnimationState animState = anim.CrossFadeQueued(ClipNames.DefaultCombatIdle, 0.3f, QueueMode.PlayNow);
			animState.wrapMode = WrapMode.Once;
		}
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
