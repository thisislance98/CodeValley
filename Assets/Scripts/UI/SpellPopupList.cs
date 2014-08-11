using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SpellType
{
	Projectile,
	Hit
}

public enum ShiftDirection
{
	up,
	down
}

public class SpellPopupList : MonoBehaviour {

	public SpellType TypeOfSpell;

	List<GameObject> _spells;
	List<string> _spellNames = new List<string>();

	UIPopupList _popup;
	// Use this for initialization
	void Start () {


		_popup = GetComponent<UIPopupList>();
	
		_spells = (TypeOfSpell == SpellType.Projectile) ? SpellManager.Instance.SpellProjectilePrefabs : SpellManager.Instance.SpellHitPrefabs;


		for (int i=0; i < _spells.Count; i++)
		{
			_spellNames.Add(_spells[i].name);

		}

		_popup.items = _spellNames;
		_popup.value = _spellNames[0];

	}

	void Update()
	{
		if (_popup.isOpen == false)
			return;

		if (Input.GetAxis("Mouse ScrollWheel") > 0 )
			Shift(ShiftDirection.up);
		else if (Input.GetAxis("Mouse ScrollWheel") < 0 )
			Shift(ShiftDirection.down);


	}

	void Shift(ShiftDirection dir)
	{
		Debug.Log("shifting " + dir);

		List<string> items = _popup.items;

		if (dir == ShiftDirection.up)
		{
			string start = items[0];
			items.RemoveAt(0);
			items.Add(start);
		}
		else if (dir == ShiftDirection.down)
		{
			string last = items[items.Count-1];
			items.RemoveAt(items.Count-1);

			items.Insert(0,last);

		}

		_popup.items = _popup.SetItems(items);


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
