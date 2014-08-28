using UnityEngine;
using System.Collections;


public class Spell : MonoBehaviour {

	public string Name;
	public string HitEffect;
	public string ProjectileEffect;
	public string Code;
	public bool CanHitTerrain;


	public void Initialize(string name, string hitEffect, string projectileEffect, string code, bool canHitTerrain)
	{
		Name = name;
		HitEffect = hitEffect;
		ProjectileEffect = projectileEffect;
		Code = code;
		CanHitTerrain = canHitTerrain;
	}

}

