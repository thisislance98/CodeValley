//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Goblin Warrior
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("")]

public class UIF_TargetResponder_GoblinWarrior : GoblinWarriorResponderBase
{
	public EffectGroupRMFX reactionPrefab;
	public bool isLethal = true;

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
			Invoke("LethalFireReaction_CALLBACK", Random.Range(0.0f, 1.25f));
		else
			Invoke("HarmlessFireReaction_CALLBACK", Random.Range(0.0f, 1.25f));
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
			victimAnim.CrossFade(ClipNames.ReactingToHit);
			Invoke("RestoreMotor_CALLBACK", victimAnim[ClipNames.ReactingToHit].length);
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

			Invoke("CorpseDisposal_CALLBACK", totalDuration + Random.Range(1.0f, 2.0f));
		}
	
		if (reactionPrefab != null)
		{
			EffectGroupRMFX fx = Instantiate(reactionPrefab, victimImplant.transform.position, Quaternion.identity) as EffectGroupRMFX;
			if (fx != null)
			{
				fx.InitEffectGroupInstance(this, "UIF Reaction (GoblinWarrior)", true/*hideChildEffects*/);
			}
		}
	}

	void RestoreMotor_CALLBACK()
	{
		if (victimImplant != null)
			victimImplant.EngageCharacterMotor();
	}

	void CorpseDisposal_CALLBACK()
	{
		if (victimImplant != null)
			Destroy(victimImplant.gameObject);
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
