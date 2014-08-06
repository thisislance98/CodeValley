using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpellType
{
	Projectile,
	Hit
}

public class SpellPopupList : MonoBehaviour {

	public SpellType TypeOfSpell;

	List<GameObject> _spells;
	List<string> _spellNames = new List<string>();

	UIPopupList _popup;
	// Use this for initialization
	void Start () {

		_popup = GetComponent<UIPopupList>();
	
		_spells = (TypeOfSpell == SpellType.Hit) ? SpellManager.Instance.SpellProjectilePrefabs : SpellManager.Instance.SpellHitPrefabs;


		for (int i=0; i < _spells.Count; i++)
		{
			_spellNames.Add(_spells[i].name);

		}

		_popup.items = _spellNames;

	}

	public void OnSpellSelected()
	{
		if (_popup == null || _popup.value == null)
			return;

		string spellName = _popup.value;

		int index = _spellNames.FindIndex(x => x.Equals(spellName));

		if (TypeOfSpell == SpellType.Hit)
			SpellManager.Instance.SetCurrentHitSpell(_spells[index]);
		else
			SpellManager.Instance.SetCurrentProjectileSpell(_spells[index]);
	}
}
