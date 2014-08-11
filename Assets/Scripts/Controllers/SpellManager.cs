using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SpellManager : MonoBehaviour {

	public GameObject SpellPrefab;

	public List<GameObject> SpellProjectilePrefabs = new List<GameObject>();
	public string ProjectileSpellsDirectory;


	public List<GameObject> SpellHitPrefabs = new List<GameObject>();
	public string HitSpellsDirectory;

	GameObject _currentProjectileSpell;
	GameObject _currentHitSpell;

	public static SpellManager Instance;


	// Use this for initialization
	void Awake () {
		Instance = this;


	}

	public GameObject CastSpell(Vector3 startPos, Vector3 targetPos, Transform target, string classTypeName)
	{
		GameObject spell = (GameObject)Instantiate(SpellPrefab,startPos,Quaternion.identity);
		GameObject spellProjectileEffect = (GameObject)Instantiate(_currentProjectileSpell);
		spellProjectileEffect.transform.parent = spell.transform;
		spellProjectileEffect.transform.localPosition = Vector3.zero;
		spell.transform.forward = (targetPos - startPos).normalized;
		
		spell.GetComponent<SpellCaster>().Initialize(targetPos,target,classTypeName,_currentHitSpell);
		return spell;
	}

	public void SetCurrentHitSpell(GameObject hitSpell)
	{
		_currentHitSpell = hitSpell;
	}

	public void SetCurrentProjectileSpell(GameObject projectileSpell)
	{
		_currentProjectileSpell = projectileSpell;

	}

	public GameObject GetCurrentHitSpell()
	{
		return _currentHitSpell;
	}

	public GameObject GetCurrentProjectileSpell()
	{
		return _currentProjectileSpell;
	}




}
