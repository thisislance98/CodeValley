using UnityEngine;
using System.Collections;

public class TerrainCheckbox : MonoBehaviour {

	public static TerrainCheckbox Instance;

	// Use this for initialization
	void Awake () {
		Instance = this;
	
	}
	

	void OnSpellLoaded(Spell spell)
	{
		gameObject.GetComponent<UIToggle>().value = spell.CanHitTerrain;
	}
}
