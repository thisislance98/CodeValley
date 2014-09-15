//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//
// ReadyMadeFX (RMFX) -- RMFX Interop Kit: Goblin Warrior
// Copyright (C) 2014 Faust Logic, Inc.  All rights reserved.
//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

using UnityEngine;

[AddComponentMenu("RMFX Interop/GoblinWarrior/Forceable Goblin Implant")]

public class ForceableGoblinImplant : ForceableObjectImplantBaseRMFX
{
	public GameObject characterMotor;
	public Collider physicalCollider;
	public Rigidbody forceableRigidbody;
	
	private IMotorRMFX liveMotor;
	private CharacterController liveController;
	private bool modifiedKinematic;
	private bool modifiedTrigger;
	
	void Start()
	{
		liveMotor = resolveCharacterMotor();
		if (liveMotor == null)
			Debug.LogWarning("Failed to find CharacterMotor.");
		
		liveController = resolveCharacterController();
				
		if (forceableRigidbody == null)
			forceableRigidbody = this.rigidbody;
		
		if (physicalCollider == null)
		{
			Collider[] colliders = GetComponents<Collider>();
			for (int i = 0; i < colliders.Length; i++)
			{
				if (!(colliders[i] is CharacterController))
				{
					physicalCollider = colliders[i];
					break;
				}
			}
		}
				
		// continuous updates not required
		this.enabled = false;
	}
	
	public override void AddExplosionForce(float power, Vector3 position, float radius, float upwardsMod, ForceMode mode)
	{
		if (forceableRigidbody != null)
		{
			if (liveMotor != null)
				liveMotor.SetSuspendedState(true);
			
			if (liveController != null)
				liveController.enabled = false;
			
			if (forceableRigidbody.isKinematic)
			{
				forceableRigidbody.isKinematic = false;
				modifiedKinematic = true;
			}
						
			if (physicalCollider != null && physicalCollider.isTrigger)
			{
				physicalCollider.isTrigger = false;
				modifiedTrigger = true;
			}
			
			Animation anim = this.GetComponent<Animation>();
			if (anim)
				anim.Play("G_war_Hit_from_front");
			
			forceableRigidbody.AddExplosionForce(power, position, radius, upwardsMod, ForceMode.Impulse);
		}
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		if (physicalCollider != null && !physicalCollider.isTrigger)
		{
			RestoreControl();
		}
	}
	
	public override void RestoreControl()
	{
		if (physicalCollider != null && modifiedTrigger)
		{
			physicalCollider.isTrigger = true;
			modifiedTrigger = false;
		}
		
		if (forceableRigidbody != null && modifiedKinematic)
		{
			forceableRigidbody.isKinematic = true;
			modifiedKinematic = false;
		}
		
		if (liveController != null)
			liveController.enabled = true;

		if (liveMotor != null)
			liveMotor.SetSuspendedState(false);
	}
	
	private IMotorRMFX resolveCharacterMotor()
	{
		// if characterMotor is specified we use that, otherwise we look for
		// an IMotorRMFX attached to this gameObject.
		GameObject motorHolder = (characterMotor != null) ? characterMotor : this.gameObject;

		// We want to generically manipulate the object's optional motor component
		// as an IMotorRMFX, but since we can't directly retrieve a component
		// using an interface type, we instead retrieve all MonoBehaviour components
		// and traverse the list testing for a component that is an IMotorRMFX.
		// The first one found is used. Attaching more than one motor component to an object
		// seems unlikely so this is probably ok.
		Component[] components = motorHolder.GetComponents<MonoBehaviour>();
		for (int i = 0; i < components.Length; i++)
		{
			IMotorRMFX motor = components[i] as IMotorRMFX;
			if (motor != null)
				return motor;
		}
		
		return null;
	}
	
	private CharacterController resolveCharacterController()
	{
		if (characterMotor != null)
			return characterMotor.GetComponent<CharacterController>();
				
		return this.gameObject.GetComponent<CharacterController>();
	}
}

//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~//~~~~~~~~~~~~~~~~~~~~~//

