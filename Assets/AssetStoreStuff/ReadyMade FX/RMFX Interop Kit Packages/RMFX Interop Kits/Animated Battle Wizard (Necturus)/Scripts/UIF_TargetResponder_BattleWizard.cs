//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class UIF_TargetResponder_BattleWizard : BattleWizardResponderBase
{
	public bool isLethal = true;
	public NamedEffectRMFX corpseDisposeFX;
	public float destructionDelay = 0f;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	private EffectResponderImplantRMFX victimImplant;
	private Animation victimAnim;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	public override bool IsValidEffectResponder(EffectResponderImplantRMFX implant, GameObject target) 
	{ 
		return implant.GetActiveEffectCountByName(effectName) == 0; 
	}

	public override void BeginEffectResponse(EffectResponderImplantRMFX implant, GameObject target, GameObject effect)
	{
		base.BeginEffectResponse(implant, target, effect);
		
		victimImplant = implant;
		if (victimImplant != null)
			victimAnim = victimImplant.GetComponent<Animation>();

		if (isLethal)
			Invoke("LethalFireReaction_CALLBACK", Random.Range(0.0f, 0.25f));
		else
			Invoke("HarmlessFireReaction_CALLBACK", Random.Range(0.0f, 0.25f));
	}

	public override Transform GetConstraintTransform(EffectResponderImplantRMFX implant, string constraintName)
	{
		switch (constraintName)
		{
		case "Anchor":
			return implant.transform.Find(HierarchyPath.Spine2);
		}

		return null;
	}

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
	// Invoke-able Callbacks

	void HarmlessFireReaction_CALLBACK()
	{
		if (victimImplant != null)
			victimImplant.DisengageCharacterMotor();

		if (victimAnim != null)
		{
			AnimationState animState;
			animState = victimAnim.CrossFadeQueued(ClipNames.ReactingToHit, 0.3f, QueueMode.PlayNow);
			animState.wrapMode = WrapMode.Once;

			Invoke("RestoreMotor_CALLBACK", animState.length);
		}
	}

	void LethalFireReaction_CALLBACK()
	{
		if (victimImplant != null)
			victimImplant.DisengageCharacterMotor();

		if (victimAnim != null)
		{
			AnimationState animState; 
			float totalDuration = 0.0f;

			animState = victimAnim.CrossFadeQueued(ClipNames.ReactingToHit, 0.3f, QueueMode.PlayNow);
			animState.wrapMode = WrapMode.Once;
			totalDuration += victimAnim[ClipNames.ReactingToHit].length;

			animState = victimAnim.CrossFadeQueued(ClipNames.FallDownDying, 0.3f, QueueMode.CompleteOthers);
			animState.wrapMode = WrapMode.ClampForever;
			totalDuration += victimAnim[ClipNames.FallDownDying].length;

			Invoke("KilledByFire_CALLBACK", totalDuration);
			Invoke("CorpseDisposal_CALLBACK", totalDuration + Random.Range(1.0f, 2.0f));
		}
	}

	void RestoreMotor_CALLBACK()
	{
		if (victimImplant != null)
			victimImplant.EngageCharacterMotor();
	}

	void KilledByFire_CALLBACK()
	{
		if (victimImplant != null)
		{
			RMFXCore.DisplayTextMessage("<i>" + victimImplant.gameObject.name + "</i>, killed by fire");
		}
	}
	
	void CorpseDisposal_CALLBACK()
	{
		if (victimImplant != null)
		{
			Destroy(victimImplant.gameObject, destructionDelay);
			
			if (corpseDisposeFX != null)
			{
				NamedEffectRMFX effectInstance = Instantiate(corpseDisposeFX) as NamedEffectRMFX;
				if (effectInstance != null)
				{
					effectInstance.effectSource = null;
					effectInstance.effectTarget = victimImplant.gameObject;
				}
			}
		}
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
