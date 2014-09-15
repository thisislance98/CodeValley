//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Battle Wizard
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class FatalImpact_TargetResponder_BattleWizard : BattleWizardResponderBase
{
	public NamedEffectRMFX corpseDisposeFX;
	public float destructionDelay = 0f;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//

	private EffectResponderImplantRMFX victimImplant;
	private Animation victimAnim;

	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
	
	public override void BeginEffectResponse(EffectResponderImplantRMFX implant, GameObject target, GameObject effect)
	{
		base.BeginEffectResponse(implant, target, effect);
		
		victimImplant = implant;
		if (victimImplant != null)
			victimAnim = victimImplant.GetComponent<Animation>();

		Invoke("LethalReaction_CALLBACK", Random.Range(0.0f, 0.5f));
	}
	
	//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//
	// Invoke-able Callbacks

	void LethalReaction_CALLBACK()
	{
		if (victimImplant != null)
			victimImplant.DisengageCharacterMotor();

		if (victimAnim != null)
		{
			string deathClip = ClipNames.FallDownDying;
			AnimationState animState = victimAnim.CrossFadeQueued(deathClip, 0.3f, QueueMode.PlayNow);
			animState.wrapMode = WrapMode.ClampForever;
			float totalDuration = victimAnim[deathClip].length;

			Invoke("KilledByImpact_CALLBACK", totalDuration);
			Invoke("CorpseDisposal_CALLBACK", totalDuration);
		}
	}
	
	void KilledByImpact_CALLBACK()
	{
		if (victimImplant != null)
		{
			RMFXCore.DisplayTextMessage("<i>" + victimImplant.gameObject.name + "</i>, killed by impact.");
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
