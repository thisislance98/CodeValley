//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Goblin Warrior
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class EQR_CastingResponder_GoblinWarrior : GoblinWarriorResponderBase
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

		totalDuration = 0.0f;

		Animation anim = liveSpellCaster.GetComponent<Animation>();
		if (anim != null)
		{
			AnimationState animState;
			
			animState = anim.CrossFadeQueued(ClipNames.ParryRightHanded, 0.3f, QueueMode.PlayNow);
			animState.wrapMode = WrapMode.Once;
			totalDuration += animState.length - 0.3f;

			animState = anim.CrossFadeQueued(ClipNames.CombatIdle, 0.3f, QueueMode.CompleteOthers);
			animState.wrapMode = WrapMode.Once;
			totalDuration += animState.length - 0.3f;
		}

		Invoke("Continue_CALLBACK", 0.48f);

		updateDelegate = Update_Active;
	}

	private void Continue_CALLBACK()
	{
		Debug.Log("SEND -- OnContinueEffect()");
		liveSpellObject.SendMessage("OnContinueEffect", SendMessageOptions.DontRequireReceiver);
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
		if (elapsedTime > totalDuration + 0.5f)
		{
			liveSpellCaster.EngageCharacterMotor();
			updateDelegate = Update_DoNothing;
		}
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
